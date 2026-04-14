using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create credentials
            ICredentials credentials = new NetworkCredential(username, password);

            // Connect to Exchange using EWS
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Build a query to find messages whose subject contains the keyword
                    ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                    queryBuilder.Subject.Contains("keyword");
                    MailQuery query = queryBuilder.GetQuery();

                    // List messages in the Inbox that match the query (recursive search)
                    ExchangeMessageInfoCollection messages = client.ListMessages(
                        client.MailboxInfo.InboxUri,
                        query);

                    // Output subject of each matching message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine(info.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during message retrieval: {ex.Message}");
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
