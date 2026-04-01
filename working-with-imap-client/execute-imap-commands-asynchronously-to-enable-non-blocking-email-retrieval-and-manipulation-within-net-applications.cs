using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Asynchronously list messages in the INBOX folder
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync("INBOX", false, null, CancellationToken.None);

                    List<MailMessage> messages = new List<MailMessage>();

                    // Fetch each message asynchronously using its UniqueId
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId, CancellationToken.None);
                        messages.Add(message);
                    }

                    // Output subjects of retrieved messages
                    foreach (MailMessage msg in messages)
                    {
                        Console.WriteLine($"Subject: {msg.Subject}");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
