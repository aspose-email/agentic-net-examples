using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and dispose the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Build a query that matches all messages
                MailQueryBuilder builder = new MailQueryBuilder();
                MailQuery query = builder.GetQuery();

                // Asynchronously retrieve the list of messages
                ImapMessageInfoCollection messages = await client.ListMessagesAsync(query, CancellationToken.None);

                Console.WriteLine($"Total messages: {messages.Count}");
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
