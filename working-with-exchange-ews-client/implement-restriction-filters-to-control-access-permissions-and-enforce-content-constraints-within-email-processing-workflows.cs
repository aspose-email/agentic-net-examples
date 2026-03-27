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
            // Set up network credentials for the Exchange server
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Initialize the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credential))
            {
                // Build a query to find messages with "confidential" in the subject line
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("confidential");
                MailQuery query = builder.GetQuery();

                // List messages in the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                // Process each message
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message using its unique URI
                    MailMessage message = client.FetchMessage(info.UniqueUri);

                    // Simple content restriction: look for disallowed keyword in the body
                    if (message.Body != null && message.Body.Contains("password"))
                    {
                        Console.WriteLine($"Restricted content detected in message: {info.Subject}");
                        // Placeholder for enforcement actions (e.g., move, delete, flag)
                    }
                    else
                    {
                        Console.WriteLine($"Message passed: {info.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream
            Console.Error.WriteLine(ex.Message);
        }
    }
}
