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
            // Initialize EWS client (replace with real values)
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Build a query to find messages received on or after a specific date
                // Using raw MailQuery syntax because ReceivedTime is not exposed as a typed property
                MailQuery query = new MailQuery("(ReceivedTime >= '10-Feb-2023')");

                // Retrieve messages from the Inbox that satisfy the query
                var messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Output basic information for each matching message
                foreach (var info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Received: {info.Date}");
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
