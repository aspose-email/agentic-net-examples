using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using System;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client
            IEWSClient client = null;
            try
            {
                // Replace with actual server URL and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                ICredentials credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(serviceUrl, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client as IDisposable)
            {
                // Build a query to filter messages from a specific sender
                var queryBuilder = new MailQueryBuilder();
                MailQuery senderQuery = queryBuilder.From.Equals("sender@example.com");

                // Retrieve the first 10 messages from the Inbox that match the sender filter
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,
                    10,
                    senderQuery);

                // Display basic information for each retrieved message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"Received: {info.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
