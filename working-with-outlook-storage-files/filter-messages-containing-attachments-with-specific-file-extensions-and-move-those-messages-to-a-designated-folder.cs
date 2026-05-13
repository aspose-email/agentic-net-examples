using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string host = "imap.example.com";
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid external calls during CI.
                if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                    return;
                }

                // Desired attachment extensions (case‑insensitive).
                List<string> targetExtensions = new List<string> { ".pdf", ".docx" };
                // Destination folder where matching messages will be moved.
                string destinationFolder = "Filtered";

                // Create and configure the IMAP client.
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Ensure the destination folder exists.
                        if (!client.ExistFolder(destinationFolder))
                        {
                            client.CreateFolder(destinationFolder);
                        }

                        // Select the source folder (INBOX).
                        client.SelectFolder("INBOX");

                        // Retrieve all messages in the selected folder.
                        ImapMessageInfoCollection messages = client.ListMessages();

                        foreach (ImapMessageInfo messageInfo in messages)
                        {
                            bool shouldMove = false;

                            // Get attachment information for the current message.
                            ImapAttachmentInfoCollection attachments = client.ListAttachments(messageInfo.SequenceNumber);

                            foreach (ImapAttachmentInfo attachment in attachments)
                            {
                                // Guard against null or empty attachment names.
                                if (string.IsNullOrEmpty(attachment.Name))
                                    continue;

                                // Check if the attachment name ends with any of the target extensions.
                                foreach (string ext in targetExtensions)
                                {
                                    if (attachment.Name.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                                    {
                                        shouldMove = true;
                                        break;
                                    }
                                }

                                if (shouldMove)
                                    break;
                            }

                            // Move the message if a matching attachment was found.
                            if (shouldMove)
                            {
                                client.MoveMessage(messageInfo.SequenceNumber, destinationFolder);
                                Console.WriteLine($"Message UID {messageInfo.UniqueId} moved to folder '{destinationFolder}'.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
