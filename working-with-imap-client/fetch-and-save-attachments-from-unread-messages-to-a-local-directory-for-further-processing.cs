using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network call in CI environments
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            // Ensure the output directory exists
            string attachmentDirectory = "Attachments";
            try
            {
                if (!Directory.Exists(attachmentDirectory))
                {
                    Directory.CreateDirectory(attachmentDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare attachment directory: {ex.Message}");
                return;
            }

            // Connect to the IMAP server
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the INBOX folder
                    client.SelectFolder(ImapFolderInfo.InBox);

                    // Retrieve all messages in the folder
                    var messagesTask = client.ListMessagesAsync();
                    messagesTask.Wait();
                    ImapMessageInfoCollection messages = messagesTask.Result;

                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        // Process only unread messages
                        if (messageInfo.IsRead)
                            continue;

                        int seqNum = messageInfo.SequenceNumber;

                        // Get attachment list for the message
                        ImapAttachmentInfoCollection attachments = client.ListAttachments(seqNum);
                        foreach (ImapAttachmentInfo attachmentInfo in attachments)
                        {
                            try
                            {
                                // Fetch the attachment
                                Attachment attachment = client.FetchAttachment(seqNum, attachmentInfo.Name);
                                // Build a safe file path
                                string safeFileName = Path.GetFileName(attachmentInfo.Name);
                                string filePath = Path.Combine(attachmentDirectory, safeFileName);
                                // Save the attachment
                                attachment.Save(filePath);
                                // Optionally mark the message as read after processing
                                client.AddMessageFlags(seqNum, ImapMessageFlags.IsRead);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to fetch/save attachment '{attachmentInfo.Name}': {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
