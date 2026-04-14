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
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("user", "pwd");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build AQS query: flagged for follow‑up and sent within the last week
                string dateString = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                string aqs = $"Isflagged:true AND SentDate >= '{dateString}'";
                ExchangeAdvancedSyntaxMailQuery query = new ExchangeAdvancedSyntaxMailQuery(aqs);

                // List messages from Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);

                // Output the unique URIs of the matching messages
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
