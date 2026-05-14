using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Configurable parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";
            long attachmentSizeThreshold = 1024 * 1024; // 1 MB

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server or credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to authenticate IMAP client: {ex.Message}");
                    return;
                }

                // Select the target folder
                try
                {
                    client.SelectFolder(folderName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder '{folderName}': {ex.Message}");
                    return;
                }

                // Retrieve messages in the folder
                IList<ImapMessageInfo> messages;
                try
                {
                    messages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                foreach (ImapMessageInfo messageInfo in messages)
                {
                    // Get attachment information for the current message
                    IList<ImapAttachmentInfo> attachments;
                    try
                    {
                        attachments = client.ListAttachments(messageInfo.SequenceNumber);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list attachments for message UID {messageInfo.UniqueId}: {ex.Message}");
                        continue;
                    }

                    bool hasLargeAttachment = false;
                    foreach (ImapAttachmentInfo attachment in attachments)
                    {
                        if (attachment.Size > attachmentSizeThreshold)
                        {
                            hasLargeAttachment = true;
                            break;
                        }
                    }

                    if (hasLargeAttachment)
                    {
                        // Add a custom flag "LargeAttachment" to the message
                        ImapMessageFlags customFlag = ImapMessageFlags.Keyword("LargeAttachment");
                        try
                        {
                            client.AddMessageFlags(messageInfo.SequenceNumber, customFlag);
                            Console.WriteLine($"Added custom flag to message UID {messageInfo.UniqueId}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add flag to message UID {messageInfo.UniqueId}: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
