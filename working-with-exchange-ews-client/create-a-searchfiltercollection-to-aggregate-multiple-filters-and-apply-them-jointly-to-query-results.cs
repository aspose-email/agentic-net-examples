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
            // Placeholder connection details
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping live EWS call.");
                return;
            }

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a composite query (equivalent to a SearchFilterCollection)
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Report");
                builder.From.Contains("test@example.com");
                MailQuery query = builder.GetQuery();

                // Apply the query to list messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Output basic information about the retrieved messages
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Received: {info.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
