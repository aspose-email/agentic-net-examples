using Aspose.Email.Storage.Pst;
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
            // Placeholder credentials – replace with real values for actual execution
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(mailboxUri) || mailboxUri.Contains("example.com") ||
                string.IsNullOrWhiteSpace(username) || username.Contains("example.com") ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operation.");
                return;
            }

            string outputDir = "Attachments";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Validate connectivity by accessing the Inbox folder
                try
                {
                    client.GetFolderInfo(client.MailboxInfo.InboxUri);
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"Failed to connect to Exchange server: {connEx.Message}");
                    return;
                }

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    if (!messageInfo.HasAttachments)
                        continue;

                    // Fetch the full message to access its attachments
                    MailMessage mail;
                    try
                    {
                        mail = client.FetchMessage(messageInfo.UniqueUri);
                    }
                    catch (Exception fetchEx)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {messageInfo.UniqueUri}: {fetchEx.Message}");
                        continue;
                    }

                    // Save each attachment preserving its original filename
                    foreach (Attachment attachment in mail.Attachments)
                    {
                        string filePath = Path.Combine(outputDir, attachment.Name);
                        try
                        {
                            attachment.Save(filePath);
                            Console.WriteLine($"Saved attachment: {filePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save attachment {attachment.Name}: {saveEx.Message}");
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
