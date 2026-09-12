using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace ExtractHighPriorityMessages
{
    // Author: Aspose.Email example
    class Program
    {
        static void Main()
        {
            const string mboxPath = "input.mbox";
            const string outputDirectory = "HighPriorityMessages";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            try
            {
                // Create MBOX reader
                using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    // Iterate through each message info
                    foreach (MboxMessageInfo messageInfo in mbox.EnumerateMessageInfo())
                    {
                        // Extract full message
                        using (MailMessage mailMessage = mbox.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions()))
                        {
                            // Check for High priority
                            if (mailMessage.Priority == MailPriority.High)
                            {
                                // Sanitize subject for filename
                                string safeSubject = MakeSafeFileName(mailMessage.Subject);
                                if (string.IsNullOrWhiteSpace(safeSubject))
                                    safeSubject = "NoSubject";

                                // Truncate to avoid overly long paths
                                if (safeSubject.Length > 100)
                                    safeSubject = safeSubject.Substring(0, 100);

                                string outputPath = Path.Combine(outputDirectory, safeSubject + ".txt");

                                // Write body to text file
                                try
                                {
                                    File.WriteAllText(outputPath, mailMessage.Body);
                                    Console.WriteLine($"Saved high‑priority message: {outputPath}");
                                }
                                catch (Exception writeEx)
                                {
                                    Console.Error.WriteLine($"Failed to write file '{outputPath}': {writeEx.Message}");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Processing complete.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
            }
        }

        // Helper to replace invalid filename characters
        private static string MakeSafeFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }
    }
}
