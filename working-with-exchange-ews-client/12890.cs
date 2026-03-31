using Aspose.Email.Clients.Exchange;
using System;
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
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client safely
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Build a query to find messages received on or after a specific date
                DateTime filterDate = DateTime.UtcNow.AddDays(-7); // e.g., last 7 days
                MailQueryBuilder builder = new MailQueryBuilder();
                // InternalDate corresponds to the ReceivedTime property
                MailQuery query = builder.InternalDate.Since(filterDate);

                // List messages from the Inbox that satisfy the query
                ExchangeMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages(client.MailboxInfo.InboxUri, query);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
                    return;
                }

                // Output basic information for each matching message
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
