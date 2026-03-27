using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Exchange server URL and credentials (replace with real values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client (implements IDisposable)
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Output some useful URIs
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                }
            }
            catch (Exception ex)
            {
                // Gracefully handle any errors
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
