using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the Exchange Web Services endpoint and user credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create a NetworkCredential instance
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client (implements IEWSClient) and ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve basic mailbox information
                var mailboxInfo = client.GetMailboxInfo();

                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                Console.WriteLine("Deleted Items URI: " + mailboxInfo.DeletedItemsUri);
                Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                Console.WriteLine("Contacts URI: " + mailboxInfo.ContactsUri);
                Console.WriteLine("Tasks URI: " + mailboxInfo.TasksUri);
            }
        }
        catch (Exception ex)
        {
            // Log any errors without crashing the application
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
