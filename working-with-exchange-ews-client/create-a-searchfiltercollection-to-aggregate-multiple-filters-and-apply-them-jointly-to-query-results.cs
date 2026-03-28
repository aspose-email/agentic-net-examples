using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the EWS client (replace placeholders with real values)
            IEWSClient client = EWSClient.GetEWSClient(
                "https://example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password"));
            using (client)
            {
                // Build a composite query (AND of multiple conditions)
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Report");
                builder.From.Contains("test@example.com");
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,
                    query,
                    false);

                // Output the unique URI of each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine(info.UniqueUri);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
