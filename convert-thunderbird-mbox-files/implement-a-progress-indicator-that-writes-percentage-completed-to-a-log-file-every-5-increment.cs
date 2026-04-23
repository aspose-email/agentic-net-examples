using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";
            string logPath = "conversion.log";

            // Ensure the log directory exists
            try
            {
                string logDir = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // Verify MBOX file existence; create minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // Write an empty MBOX file (no messages)
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Determine total number of messages for progress calculation
            int totalMessages;
            try
            {
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    totalMessages = mboxReader.GetTotalItemsCount();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read MBOX file: {ex.Message}");
                return;
            }

            if (totalMessages == 0)
            {
                Console.WriteLine("MBOX file contains no messages. Conversion skipped.");
                return;
            }

            int processedCount = 0;
            int lastLoggedPercent = 0;

            // Open log writer once for the whole conversion
            using (StreamWriter logWriter = new StreamWriter(logPath, true))
            {
                // Define the progress handler
                MailStorageConverter.MailHandler progressHandler = (MailMessage message) =>
                {
                    processedCount++;
                    int percent = (processedCount * 100) / totalMessages;
                    if (percent >= lastLoggedPercent + 5 || percent == 100)
                    {
                        logWriter.WriteLine($"{DateTime.Now}: {percent}% completed.");
                        logWriter.Flush();
                        lastLoggedPercent = percent;
                    }
                };

                // Perform the conversion with the progress handler
                try
                {
                    MailStorageConverter.MboxToPst(mboxPath, pstPath, progressHandler);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                    return;
                }

                // Ensure final 100% is logged if not already
                if (lastLoggedPercent < 100)
                {
                    logWriter.WriteLine($"{DateTime.Now}: 100% completed.");
                }
            }

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
