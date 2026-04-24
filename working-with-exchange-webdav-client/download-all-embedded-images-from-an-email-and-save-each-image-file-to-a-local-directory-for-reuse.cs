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
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username@example.com";
            string password = "password";

            // Guard against running with placeholder credentials.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Directory where extracted images will be saved.
            string outputDir = "ExtractedImages";

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to Exchange using WebDav client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List messages in the Inbox with attachment information.
                ExchangeMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages(client.MailboxInfo.InboxUri,
                        ExchangeListMessagesOptions.FetchAttachmentInformation);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Fetch the full mail message.
                    MailMessage mailMessage;
                    try
                    {
                        mailMessage = client.FetchMessage(messageInfo.UniqueUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message '{messageInfo.Subject}': {ex.Message}");
                        continue;
                    }

                    using (mailMessage)
                    {
                        // Iterate over attachments and extract inline (embedded) images.
                        foreach (Attachment attachment in mailMessage.Attachments)
                        {
                            // An embedded image typically has a ContentId or an inline disposition.
                            bool isInline = !string.IsNullOrEmpty(attachment.ContentId) ||
                                            (attachment.ContentDisposition != null &&
                                             string.Equals(attachment.ContentDisposition.DispositionType,
                                                           "inline",
                                                           StringComparison.OrdinalIgnoreCase));

                            if (!isInline)
                                continue;

                            // Determine a safe file name.
                            string fileName = !string.IsNullOrEmpty(attachment.Name)
                                ? attachment.Name
                                : $"{Guid.NewGuid()}.dat";

                            string filePath = Path.Combine(outputDir, fileName);

                            // Save the attachment to disk.
                            try
                            {
                                attachment.Save(filePath);
                                Console.WriteLine($"Saved embedded image: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{fileName}': {ex.Message}");
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
