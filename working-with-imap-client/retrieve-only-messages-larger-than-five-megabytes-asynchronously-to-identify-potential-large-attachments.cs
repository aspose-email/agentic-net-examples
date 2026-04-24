using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Tools;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Validate credentials safely.
                try
                {
                    await client.ValidateCredentialsAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication failed: {ex.Message}");
                    return;
                }

                // Select the INBOX folder.
                await client.SelectFolderAsync("INBOX", CancellationToken.None);

                // Retrieve all messages in the folder.
                ImapMessageInfoCollection allMessages = await client.ListMessagesAsync(CancellationToken.None);

                // Filter messages larger than 5 MB.
                const long fiveMegabytes = 5L * 1024 * 1024;
                List<ImapMessageInfo> largeMessages = new List<ImapMessageInfo>();
                foreach (ImapMessageInfo info in allMessages)
                {
                    if (info.Size > fiveMegabytes)
                    {
                        largeMessages.Add(info);
                    }
                }

                // Output identifiers of large messages.
                Console.WriteLine($"Found {largeMessages.Count} message(s) larger than 5 MB:");
                foreach (ImapMessageInfo info in largeMessages)
                {
                    Console.WriteLine($"- UID: {info.UniqueId}, Size: {info.Size} bytes, Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
