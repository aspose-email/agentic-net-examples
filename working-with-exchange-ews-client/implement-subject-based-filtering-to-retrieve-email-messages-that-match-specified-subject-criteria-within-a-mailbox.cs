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
            // Define connection parameters (replace with real values as needed)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the EWS client within a using block for proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Build a subject-based query (e.g., messages containing "Invoice")
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);

                    // Iterate through the results and display each message's subject
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Fetch the full message to access its properties
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
                catch (Exception ex)
                {
                    // Handle client operation errors gracefully
                    Console.Error.WriteLine("Error during EWS operations: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle initialization errors (e.g., network issues, authentication failures)
            Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
        }
    }
}
