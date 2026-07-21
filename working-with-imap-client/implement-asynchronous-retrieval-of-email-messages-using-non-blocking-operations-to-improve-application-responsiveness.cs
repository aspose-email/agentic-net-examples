using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsyncImapExample
{
    class Program
    {
        // Author note: Demonstrates non‑blocking retrieval of messages from an IMAP server.
        static async Task Main(string[] args)
        {
            try
            {
                // Connection parameters – replace with real credentials.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Wrap the client in a using block to ensure proper disposal.
                using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Asynchronously get the list of messages in the INBOX folder.
                        ImapMessageInfoCollection messageInfos = await imapClient.ListMessagesAsync("INBOX");

                        // Extract sequence numbers for the messages we want to fetch.
                        List<int> sequenceNumbers = new List<int>();
                        foreach (ImapMessageInfo info in messageInfos)
                        {
                            sequenceNumbers.Add(info.SequenceNumber);
                        }

                        // Asynchronously fetch the full MailMessage objects.
                        IList<MailMessage> messages = await imapClient.FetchMessagesAsync(sequenceNumbers);

                        // Process the retrieved messages (here we simply output subjects).
                        foreach (MailMessage message in messages)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
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
