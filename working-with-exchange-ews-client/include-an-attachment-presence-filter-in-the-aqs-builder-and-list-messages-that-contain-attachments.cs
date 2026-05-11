using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
class Program
{
    static void Main()
    {
        try
        {
            // Server URI and credentials
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serverUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(serverUri, new NetworkCredential(username, password)))
            {
                // Build an AQS query to find messages that have attachments
                ExchangeAdvancedSyntaxQueryBuilder builder = new ExchangeAdvancedSyntaxQueryBuilder();
                builder.HasAttachment.Equals("true");
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query (contain attachments)
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);

                // Output the subject of each message
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
