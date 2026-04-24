using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholders are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create a temporary folder for extracted attachments
            string tempFolder = Path.Combine(Path.GetTempPath(), "ImapAttachments");
            try
            {
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create temporary folder: {ex.Message}");
                return;
            }

            // Connect to the IMAP server
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password))
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the selected folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        // Get attachments for the current message
                        ImapAttachmentInfoCollection attachmentsInfo = client.ListAttachments(messageInfo.SequenceNumber);
                        foreach (ImapAttachmentInfo attachmentInfo in attachmentsInfo)
                        {
                            // Fetch the attachment as an Aspose.Email.Attachment
                            Attachment attachment = client.FetchAttachment(messageInfo.SequenceNumber, attachmentInfo.Name);
                            if (attachment == null || attachment.ContentStream == null)
                                continue;

                            // Build a safe file path
                            string safeFileName = Path.GetFileName(attachmentInfo.Name);
                            string outputPath = Path.Combine(tempFolder, safeFileName);

                            // Write the attachment stream to the file
                            try
                            {
                                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                                {
                                    attachment.ContentStream.CopyTo(fileStream);
                                }
                                Console.WriteLine($"Saved attachment: {outputPath}");
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {ioEx.Message}");
                            }
                            finally
                            {
                                attachment.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception connEx)
            {
                Console.Error.WriteLine($"IMAP operation failed: {connEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
