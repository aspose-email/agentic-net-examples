using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths
            const string mboxPath = "input.mbox";
            const string outputFolder = "ExtractedMessages";
            const string logPath = "progress.log";

            // Guard file system access
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            try
            {
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Initialize log file
            try
            {
                File.WriteAllText(logPath, string.Empty);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to initialize log file: {ex.Message}");
                return;
            }

            // Read MBOX storage
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Collect all message infos to determine total count
                List<MboxMessageInfo> allInfos = mbox.EnumerateMessageInfo().ToList();
                int total = allInfos.Count;
                if (total == 0)
                {
                    Console.WriteLine("No messages found in the MBOX file.");
                    return;
                }

                int lastLoggedPercent = -5; // ensures first log at 0%
                for (int i = 0; i < total; i++)
                {
                    MboxMessageInfo info = allInfos[i];
                    // Extract full MIME message
                    MailMessage eml = mbox.ExtractMessage(info.EntryId, new EmlLoadOptions());

                    // Save as .eml file (sanitize file name)
                    string safeSubject = string.Concat(eml.Subject.Split(Path.GetInvalidFileNameChars()));
                    string emlPath = Path.Combine(outputFolder, $"{safeSubject}_{i + 1}.eml");
                    try
                    {
                        eml.Save(emlPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message '{eml.Subject}': {ex.Message}");
                        // Continue processing other messages
                    }

                    // Progress calculation
                    int percent = (i + 1) * 100 / total;
                    if (percent % 5 == 0 && percent != lastLoggedPercent)
                    {
                        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Processed {percent}% of messages.";
                        try
                        {
                            File.AppendAllText(logPath, logEntry + Environment.NewLine);
                        }
                        catch
                        {
                            // Swallow logging errors to avoid breaking processing
                        }
                        lastLoggedPercent = percent;
                    }
                }
            }

            Console.WriteLine("MBOX processing completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
