using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials check – skip real network calls in CI environments
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client within a using block to ensure disposal
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Select the INBOX folder (implicit connection)
                await client.SelectFolderAsync("INBOX");

                // Retrieve basic information about all messages in the folder
                ImapMessageInfoCollection infos = await client.ListMessagesAsync();

                if (infos == null || infos.Count == 0)
                {
                    Console.WriteLine("No messages found in the INBOX.");
                    return;
                }

                // Prepare sequence numbers for bulk fetch
                List<int> sequenceNumbers = infos.Select(info => info.SequenceNumber).ToList();

                // Fetch the full MailMessage objects asynchronously
                IList<MailMessage> messages = await client.FetchMessagesAsync(sequenceNumbers);

                // Ensure we have matching counts
                if (messages.Count != infos.Count)
                {
                    Console.WriteLine("Mismatch between fetched messages and info collection.");
                    return;
                }

                // Iterate and calculate latency (Sent - Received)
                for (int i = 0; i < messages.Count; i++)
                {
                    MailMessage mail = messages[i];
                    ImapMessageInfo info = infos[i];

                    DateTime sentDate = mail.Date;               // Date header (when the message was sent)
                    DateTime receivedDate = info.InternalDate;   // Server's internal receipt date

                    TimeSpan latency = sentDate - receivedDate;

                    Console.WriteLine($"Message UID: {info.UniqueId}");
                    Console.WriteLine($"  Sent:     {sentDate:u}");
                    Console.WriteLine($"  Received: {receivedDate:u}");
                    Console.WriteLine($"  Latency:  {latency.TotalSeconds:F2} seconds");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
