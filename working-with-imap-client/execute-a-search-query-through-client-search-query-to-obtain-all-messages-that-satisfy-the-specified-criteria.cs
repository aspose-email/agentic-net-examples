using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select the folder to search
                client.SelectFolder("INBOX");

                // Build the search query (e.g., messages with "Report" in the subject)
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Report");
                MailQuery query = builder.GetQuery();

                // Execute the search
                ImapMessageInfoCollection results = client.ListMessages(query);

                // Iterate over the results and fetch each message
                foreach (ImapMessageInfo info in results)
                {
                    using (MailMessage message = client.FetchMessage(info.UniqueId))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}