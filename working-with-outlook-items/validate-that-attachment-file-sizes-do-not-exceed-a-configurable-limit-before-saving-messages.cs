using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Configurable maximum attachment size (5 MB)
            const long maxAttachmentSizeBytes = 5 * 1024 * 1024;

            // Exchange server connection details (placeholders)
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip network operations when placeholders are detected
            if (exchangeUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Folder where messages will be saved
            string outputFolder = "SavedMessages";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Create and use the Exchange client
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // Verify connectivity by listing messages in the Inbox
                    try
                    {
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            // Fetch the full mail message
                            using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                            {
                                bool attachmentsOk = true;

                                // Validate each attachment size
                                foreach (Attachment attachment in message.Attachments)
                                {
                                    try
                                    {
                                        if (attachment.ContentStream != null)
                                        {
                                            long size = attachment.ContentStream.Length;
                                            if (size > maxAttachmentSizeBytes)
                                            {
                                                Console.WriteLine($"Skipping message \"{message.Subject}\" because attachment \"{attachment.Name}\" size {size} exceeds limit.");
                                                attachmentsOk = false;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine($"Error checking attachment size: {ex.Message}");
                                        attachmentsOk = false;
                                        break;
                                    }
                                }

                                if (!attachmentsOk)
                                {
                                    continue;
                                }

                                // Build a safe file name from the subject
                                string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(c, '_');
                                }
                                string filePath = Path.Combine(outputFolder, safeSubject + ".eml");

                                // Save the message to the file system
                                try
                                {
                                    client.SaveMessage(messageInfo.UniqueUri, filePath);
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to save message \"{message.Subject}\": {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error accessing mailbox: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or use Exchange client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
