using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "P@ssw0rd";

                // Create credentials object
                NetworkCredential credentials = new NetworkCredential(username, password);

                // Initialize the EWS client (implements IEWSClient)
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Optional: set a timeout (in milliseconds)
                    ewsClient.Timeout = 120000; // 2 minutes

                    // Example operation: retrieve mailbox information
                    var mailboxInfo = ewsClient.GetMailboxInfo();

                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
