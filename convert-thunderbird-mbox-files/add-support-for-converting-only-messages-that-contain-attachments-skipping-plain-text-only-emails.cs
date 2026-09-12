using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "source.mbox";
            // Output directory for messages that have attachments
            string outputDir = "output";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create MBOX reader
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
                {
                    // Extract the full MIME message
                    using (MailMessage message = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions()))
                    {
                        // Process only messages that contain at least one attachment
                        if (message.Attachments.Count > 0)
                        {
                            // Build a safe file name for the output message
                            string safeFileName = $"{Guid.NewGuid()}.eml";
                            string outPath = Path.Combine(outputDir, safeFileName);

                            // Preserve embedded message format while saving
                            EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                            {
                                PreserveEmbeddedMessageFormat = true
                            };

                            // Save the message
                            message.Save(outPath, saveOptions);
                            Console.WriteLine($"Saved message with attachments: {outPath}");
                        }
                        else
                        {
                            Console.WriteLine($"Skipped plain‑text message: {mboxMessageInfo.Subject}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
