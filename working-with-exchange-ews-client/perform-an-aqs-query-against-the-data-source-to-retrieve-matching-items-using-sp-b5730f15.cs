using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define mailbox URI and credentials (replace with actual values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Build an AQS query: subject contains "Report" and from contains "alice@example.com"
                    ExchangeAdvancedSyntaxQueryBuilder builder = new ExchangeAdvancedSyntaxQueryBuilder();
                    builder.Subject.Contains("Report");
                    builder.From.Contains("alice@example.com");
                    MailQuery query = builder.GetQuery();

                    // List messages in the Inbox folder that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    // Output basic metadata of each matching message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("From: " + info.From);
                        Console.WriteLine("Received: " + info.Date);
                        Console.WriteLine("Unique URI: " + info.UniqueUri);
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during EWS operations: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}