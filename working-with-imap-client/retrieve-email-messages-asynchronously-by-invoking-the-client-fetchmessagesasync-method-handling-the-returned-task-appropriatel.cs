using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main()
    {
        try
        {
            // Connection settings
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

            // Create and use the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // List messages in the INBOX folder asynchronously
                ImapMessageInfoCollection messageInfos = await imapClient.ListMessagesAsync("INBOX");

                // Gather sequence numbers of the messages
                List<int> sequenceNumbers = new List<int>();
                foreach (ImapMessageInfo info in messageInfos)
                {
                    sequenceNumbers.Add(info.SequenceNumber);
                }

                // Fetch the messages asynchronously using the client
                IList<MailMessage> messages = await imapClient.FetchMessagesAsync(sequenceNumbers);

                // Example processing: output each message subject
                foreach (MailMessage message in messages)
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
