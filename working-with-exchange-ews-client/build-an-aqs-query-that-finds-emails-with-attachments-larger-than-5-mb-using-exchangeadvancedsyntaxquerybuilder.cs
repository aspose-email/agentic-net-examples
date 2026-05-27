using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder Exchange server details – replace with real values when running against a real server
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Detect placeholder values and skip actual network calls
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Instantiate the builder (required by the task) – the actual query will be built manually
                ExchangeAdvancedSyntaxQueryBuilder queryBuilder = new ExchangeAdvancedSyntaxQueryBuilder();

                // Build AQS query: messages that have an attachment and size > 5 MB
                // AQS syntax: hasattachment:true AND size:>5242880
                string aqsString = "hasattachment:true AND size:>5242880";
                MailQuery aqsQuery = new MailQuery(aqsString);

                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve messages that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, aqsQuery);

                // Output the subject of each matching message
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine("Subject: " + messageInfo.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
