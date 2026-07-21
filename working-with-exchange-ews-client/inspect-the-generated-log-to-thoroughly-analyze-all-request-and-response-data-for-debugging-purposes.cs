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
            // Define connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client inside a using block to ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
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
            // Log any unexpected errors without crashing the application
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
