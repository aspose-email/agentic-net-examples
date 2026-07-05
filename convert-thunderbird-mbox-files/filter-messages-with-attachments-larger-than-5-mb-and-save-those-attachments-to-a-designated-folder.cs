using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        const string mboxPath = "input.mbox";
        const string outputFolder = "LargeAttachments";
        const long sizeThreshold = 5L * 1024 * 1024; // 5 MB

        // Verify input MBOX file exists
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
            return;
        }

        // Ensure output directory exists
        try
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
            return;
        }

        try
        {
            // Open the MBOX storage
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
                {
                    // Extract the full MIME message
                    using (MailMessage message = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions()))
                    {
                        // Process each attachment
                        foreach (Attachment attachment in message.Attachments)
                        {
                            long attachmentSize = 0;
                            if (attachment.ContentStream != null && attachment.ContentStream.CanSeek)
                            {
                                attachmentSize = attachment.ContentStream.Length;
                            }

                            if (attachmentSize > sizeThreshold)
                            {
                                string safeFileName = !string.IsNullOrEmpty(attachment.Name) ? attachment.Name : "attachment.bin";

                                string targetPath = Path.Combine(outputFolder, safeFileName);
                                int duplicateIndex = 1;
                                while (File.Exists(targetPath))
                                {
                                    string fileNameOnly = Path.GetFileNameWithoutExtension(safeFileName);
                                    string extension = Path.GetExtension(safeFileName);
                                    targetPath = Path.Combine(outputFolder, $"{fileNameOnly}_{duplicateIndex}{extension}");
                                    duplicateIndex++;
                                }

                                try
                                {
                                    attachment.Save(targetPath);
                                    Console.WriteLine($"Saved large attachment: {targetPath} ({attachmentSize} bytes)");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {saveEx.Message}");
                                }
                            }
                        }
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
