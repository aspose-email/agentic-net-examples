using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

namespace ExchangeWebDavSample
{
    class Program
    {
        static void Main()
        {
            // Exchange server connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/exchange";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip network operations when placeholder values are detected
            bool isPlaceholder = mailboxUri.Contains("example.com") ||
                                 username.Contains("example.com") ||
                                 password == "password";

            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Ensure the output directory exists before any file operations
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Establish a WebDAV connection using ExchangeClient
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information to get standard folder URIs
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection inboxMessages = client.ListMessages(mailboxInfo.InboxUri);

                    foreach (ExchangeMessageInfo msgInfo in inboxMessages)
                    {
                        try
                        {
                            // Fetch the full mail message
                            MailMessage message = client.FetchMessage(msgInfo.UniqueUri);

                            // Build a safe file name for the message
                            string safeSubject = string.IsNullOrWhiteSpace(msgInfo.Subject) ? "NoSubject" : msgInfo.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }
                            string emlPath = Path.Combine(outputDir, safeSubject + ".eml");

                            // Save the message to disk (overwrite if it already exists)
                            message.Save(emlPath, SaveOptions.DefaultEml);

                            // Move the processed message to Deleted Items folder
                            client.MoveMessage(msgInfo, mailboxInfo.DeletedItemsUri);
                        }
                        catch (Exception exMsg)
                        {
                            Console.Error.WriteLine($"Error processing message '{msgInfo.Subject}': {exMsg.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
            }
        }
    }
}
