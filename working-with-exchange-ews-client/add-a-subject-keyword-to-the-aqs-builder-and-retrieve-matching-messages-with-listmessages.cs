using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Create and connect to the Exchange server using EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build an AQS query that searches for a keyword in the subject
                ExchangeAdvancedSyntaxQueryBuilder aqsBuilder = new ExchangeAdvancedSyntaxQueryBuilder();
                aqsBuilder.Subject.Contains("Invoice");
                MailQuery query = aqsBuilder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Output the subject of each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
