using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder connection settings detected. Skipping execution.");
                return;
            }

            // Directory where attachments will be saved
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Attachments");

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Create and use the Exchange WebDAV client
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Get the Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List messages with attachment information
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, ExchangeListMessagesOptions.FetchAttachmentInformation);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        if (messageInfo.HasAttachments && messageInfo.Attachments != null)
                        {
                            foreach (ExchangeAttachmentInfo attachmentInfo in messageInfo.Attachments)
                            {
                                // Fetch the attachment
                                using (Attachment attachment = client.FetchAttachment(attachmentInfo.AttachmentUri))
                                {
                                    // Build a safe file name
                                    string safeFileName = string.Join("_", attachmentInfo.Name.Split(Path.GetInvalidFileNameChars()));
                                    string filePath = Path.Combine(outputDirectory, safeFileName);

                                    // Save the attachment content to disk
                                    try
                                    {
                                        using (FileStream fileStream = File.Create(filePath))
                                        {
                                            attachment.ContentStream.CopyTo(fileStream);
                                        }
                                        Console.WriteLine($"Saved attachment: {filePath}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine($"Failed to save attachment '{attachmentInfo.Name}': {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
