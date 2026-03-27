using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Initialize EWS client with placeholder credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Enable tracing/logging by specifying a log file.
                // The TraceEnabled property is not available in this version;
                // using LogFileName and UseDateInLogFileName provides similar tracing.
                client.LogFileName = "ews_trace.log";
                client.UseDateInLogFileName = true;

                // Example operation to verify the client works (list inbox messages)
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                Console.WriteLine($"Retrieved {messages.Count} messages from the inbox.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}