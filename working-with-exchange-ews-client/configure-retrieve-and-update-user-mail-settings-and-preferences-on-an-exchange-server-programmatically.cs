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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create network credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Output some mailbox URIs (these are valid members of ExchangeMailboxInfo)
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                    Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);

                    // Update a user preference – set the timezone identifier
                    client.TimezoneId = "Pacific Standard Time";
                    Console.WriteLine("Timezone updated to: " + client.TimezoneId);
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during client operations
                    Console.Error.WriteLine("Error during Exchange operations: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
