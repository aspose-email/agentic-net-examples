using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Initialize the IMAP client with host, port, credentials and security options.
            using (ImapClient imapClient = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
            {
                // Select the INBOX folder.
                imapClient.SelectFolder("INBOX");

                // Retrieve the list of messages in the selected folder.
                ImapMessageInfoCollection messageInfos = await imapClient.ListMessagesAsync();

                // Collect the sequence numbers of the messages.
                List<int> sequenceNumbers = new List<int>();
                foreach (ImapMessageInfo info in messageInfos)
                {
                    sequenceNumbers.Add(info.SequenceNumber);
                }

                // Asynchronously fetch the full messages using the sequence numbers.
                IList<MailMessage> messages = await imapClient.FetchMessagesAsync(sequenceNumbers);

                // Output the subject of each fetched message.
                foreach (MailMessage message in messages)
                {
                    Console.WriteLine("Subject: " + message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}