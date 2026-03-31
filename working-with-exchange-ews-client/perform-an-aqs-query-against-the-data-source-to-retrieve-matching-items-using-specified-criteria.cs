using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Build an AQS query (e.g., find messages with subject containing "Report")
                    ExchangeAdvancedSyntaxQueryBuilder builder = new ExchangeAdvancedSyntaxQueryBuilder();
                    builder.Subject.Contains("Report");
                    MailQuery query = builder.GetQuery();

                    // Perform the query against the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    // Output the subjects of the matching messages
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during query execution: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
