using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mime;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – replace with real values for actual execution.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Detect placeholder credentials and skip network operations.
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Folder where suspicious messages will be moved.
                string quarantineFolder = "Quarantine";

                // List of attachment extensions that trigger quarantine.
                string[] suspiciousExtensions = new string[] { ".exe", ".js", ".vbs" };

                // Create and connect the IMAP client.
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    client.Username = username;
                    client.Password = password;

                    // Verify connection credentials.
                    try
                    {
                        client.ValidateCredentials();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Authentication failed: {ex.Message}");
                        return;
                    }

                    // Ensure the quarantine folder exists.
                    bool quarantineExists;
                    try
                    {
                        quarantineExists = client.ExistFolder(quarantineFolder);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to check folder existence: {ex.Message}");
                        return;
                    }

                    if (!quarantineExists)
                    {
                        try
                        {
                            client.CreateFolder(quarantineFolder);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create quarantine folder: {ex.Message}");
                            return;
                        }
                    }

                    // Select the INBOX folder.
                    try
                    {
                        client.SelectFolder("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to select INBOX: {ex.Message}");
                        return;
                    }

                    // Retrieve messages from INBOX.
                    ImapMessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                        return;
                    }

                    foreach (ImapMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full message to inspect attachments.
                        MailMessage mailMessage;
                        try
                        {
                            mailMessage = client.FetchMessage(messageInfo.UniqueId);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {messageInfo.UniqueId}: {ex.Message}");
                            continue;
                        }

                        using (mailMessage)
                        {
                            bool hasSuspiciousAttachment = false;

                            foreach (Attachment attachment in mailMessage.Attachments)
                            {
                                string extension = Path.GetExtension(attachment.Name);
                                if (Array.Exists(suspiciousExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                                {
                                    hasSuspiciousAttachment = true;
                                    break;
                                }
                            }

                            if (hasSuspiciousAttachment)
                            {
                                try
                                {
                                    // Move the message to the quarantine folder.
                                    client.MoveMessage(quarantineFolder, messageInfo.UniqueId);
                                    Console.WriteLine($"Message {messageInfo.UniqueId} moved to quarantine.");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to move message {messageInfo.UniqueId}: {ex.Message}");
                                }
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
}
