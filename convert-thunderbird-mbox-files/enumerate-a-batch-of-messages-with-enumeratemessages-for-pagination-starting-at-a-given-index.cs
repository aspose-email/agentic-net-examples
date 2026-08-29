using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxBatchEnumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input MBOX file path
            const string mboxPath = "storage.mbox";
            // Output directory for extracted messages
            const string outputDir = "output";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                return;
            }

            // Pagination parameters
            const int startIndex = 0;   // zero‑based start index
            const int batchSize = 10;   // number of messages to retrieve

            try
            {
                // Create the MBOX reader
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    // Enumerate a batch of messages
                    foreach (MailMessage message in reader.EnumerateMessages(startIndex, batchSize))
                    {
                        // Sanitize subject for filename
                        string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                        string safeSubject = new string(subject.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '_' : ch).ToArray());

                        // Build unique file path
                        string fileName = $"{safeSubject}_{Guid.NewGuid()}.eml";
                        string filePath = Path.Combine(outputDir, fileName);

                        try
                        {
                            // Save the message as .eml
                            message.Save(filePath);
                            Console.WriteLine($"Saved: {filePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message '{subject}': {saveEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
            }
        }
    }
}
