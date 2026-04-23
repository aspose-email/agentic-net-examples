using System;
using System.IO;
using System.Linq;
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

            // Ensure the MBOX file exists; create a minimal placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Guard PST output path directory.
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory for PST file: {ex.Message}");
                    return;
                }
            }

            // Count total messages in the MBOX to calculate progress.
            int totalMessages;
            try
            {
                using (MboxStorageReader countReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    totalMessages = countReader.EnumerateMessageInfo().Count();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while counting messages: {ex.Message}");
                return;
            }

            if (totalMessages == 0)
            {
                Console.WriteLine("No messages found in the MBOX file. Conversion skipped.");
                return;
            }

            int processedCount = 0;

            // Set up conversion options with a progress handler.
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.MessageHandler = (MailMessage message) =>
            {
                processedCount++;
                int percent = (int)((processedCount / (double)totalMessages) * 100);
                Console.WriteLine($"Conversion progress: {percent}% ({processedCount}/{totalMessages})");
            };

            // Perform the conversion.
            try
            {
                MailStorageConverter.MboxToPst(mboxPath, pstPath, options);
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
