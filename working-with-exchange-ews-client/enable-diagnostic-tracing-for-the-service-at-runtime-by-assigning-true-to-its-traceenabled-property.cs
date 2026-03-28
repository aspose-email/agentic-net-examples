using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client using the factory method.
            // Replace the placeholder values with actual server URL and credentials.
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // The IEWSClient type does not expose a TraceEnabled property.
                // Diagnostic information can be captured by specifying a log file name if needed.
                // client.LogFileName = "ews_log.txt";

                // Example operation: list subjects of messages in the Inbox folder.
                foreach (ExchangeMessageInfo info in client.ListMessages(client.MailboxInfo.InboxUri))
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
