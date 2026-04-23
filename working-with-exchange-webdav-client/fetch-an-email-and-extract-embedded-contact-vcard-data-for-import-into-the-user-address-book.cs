using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder credentials are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Validate connection by attempting to list messages in the Inbox
                ExchangeMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                foreach (ExchangeMessageInfo messageInfo in messageInfos)
                {
                    // Fetch the full mail message
                    using (MailMessage mail = client.FetchMessage(messageInfo.UniqueUri))
                    {
                        foreach (Attachment attachment in mail.Attachments)
                        {
                            // Identify vCard attachments
                            bool isVCard = attachment.ContentType.MediaType.Equals("text/vcard", StringComparison.OrdinalIgnoreCase) ||
                                           attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase);
                            if (!isVCard) continue;

                            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.Name);

                            // Ensure the output directory exists
                            try
                            {
                                string dir = Path.GetDirectoryName(outputPath);
                                if (!Directory.Exists(dir))
                                    Directory.CreateDirectory(dir);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to ensure directory for '{outputPath}': {ex.Message}");
                                continue;
                            }

                            // Save the vCard attachment to disk
                            try
                            {
                                using (Stream source = attachment.ContentStream)
                                using (FileStream dest = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                                {
                                    source.CopyTo(dest);
                                }
                                Console.WriteLine($"Saved vCard to '{outputPath}'.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save vCard '{attachment.Name}': {ex.Message}");
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
