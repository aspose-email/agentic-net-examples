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
            // Paths for the source MBOX file and the destination folder.
            string mboxPath = "input.mbox";
            string outputFolder = "LargeAttachments";

            // Verify that the MBOX file exists.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Open the MBOX storage for reading.
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Iterate through each message in the MBOX file.
                foreach (MailMessage message in mboxReader.EnumerateMessages())
                {
                    // Iterate through each attachment of the current message.
                    foreach (Attachment attachment in message.Attachments)
                    {
                        // Some attachments may not have a content stream; skip those.
                        if (attachment.ContentStream == null)
                            continue;

                        // Check if the attachment size exceeds 5 MB.
                        const long FiveMegabytes = 5L * 1024 * 1024;
                        if (attachment.ContentStream.Length > FiveMegabytes)
                        {
                            // Determine a safe file name for the attachment.
                            string safeFileName = Path.GetFileName(attachment.Name);
                            if (string.IsNullOrEmpty(safeFileName))
                                safeFileName = "attachment.bin";

                            string destinationPath = Path.Combine(outputFolder, safeFileName);

                            // Save the attachment to the designated folder.
                            using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.ContentStream.Position = 0;
                                attachment.ContentStream.CopyTo(fileStream);
                            }

                            Console.WriteLine($"Saved large attachment: {destinationPath}");
                        }
                    }

                    // Dispose the message after processing.
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
