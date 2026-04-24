using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailSpamFilter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder credentials are detected
                if (host.Contains("example.com") || username.Contains("example.com") || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and use the IMAP client
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Validate credentials (connection and authentication)
                        await client.ValidateCredentialsAsync();

                        // Select the INBOX folder
                        await client.SelectFolderAsync("INBOX");

                        // Retrieve all messages in the INBOX
                        IList<ImapMessageInfo> messageInfos = await client.ListMessagesAsync();

                        foreach (ImapMessageInfo messageInfo in messageInfos)
                        {
                            // Fetch the full message to examine its subject
                            MailMessage mailMessage = await client.FetchMessageAsync(messageInfo.UniqueId);

                            // Simple spam detection: subject contains the word "spam"
                            if (mailMessage.Subject != null && mailMessage.Subject.IndexOf("spam", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                // Move the flagged message to the Spam folder
                                await client.MoveMessageAsync(messageInfo.UniqueId, "Spam");
                                Console.WriteLine($"Moved message UID {messageInfo.UniqueId} to Spam folder.");
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
            }
        }
    }
}
