using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "storage.mbox";

            // Output directory for extracted EML files
            string outputDir = "ExtractedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Verify the MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Create the MBOX reader
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Get total number of messages for progress calculation
                int totalMessages = mboxReader.EnumerateMessageInfo().Count();

                if (totalMessages == 0)
                {
                    Console.WriteLine("No messages found in the MBOX file.");
                    return;
                }

                int processed = 0;

                // Iterate through each message info object
                foreach (Aspose.Email.Storage.Mbox.MboxMessageInfo messageInfo in mboxReader.EnumerateMessageInfo())
                {
                    // Extract the full MIME message using required EmlLoadOptions
                    MailMessage emlMessage = mboxReader.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions());

                    // Build a safe file name from the subject
                    string safeSubject = string.Join("_", emlMessage.Subject.Split(Path.GetInvalidFileNameChars()));
                    if (string.IsNullOrWhiteSpace(safeSubject))
                    {
                        safeSubject = "Untitled";
                    }

                    string emlPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                    // Save the extracted message
                    try
                    {
                        emlMessage.Save(emlPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message '{emlMessage.Subject}': {ex.Message}");
                        // Continue processing remaining messages
                    }

                    // Update progress
                    processed++;
                    int percent = (int)((processed / (double)totalMessages) * 100);
                    ReportProgress(percent);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple progress reporter (could be bound to a UI progress bar)
    static void ReportProgress(int percent)
    {
        Console.WriteLine($"Progress: {percent}%");
    }
}
