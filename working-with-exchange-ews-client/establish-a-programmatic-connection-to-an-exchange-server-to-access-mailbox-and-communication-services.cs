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
            // Connection parameters – replace with your actual server details
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create network credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client (IEWSClient implements IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Optional: adjust timeout (milliseconds)
                client.Timeout = 120000; // 2 minutes

                // Retrieve basic mailbox information
                var mailboxInfo = client.GetMailboxInfo();

                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                Console.WriteLine("Contacts URI: " + mailboxInfo.ContactsUri);
                Console.WriteLine("Tasks URI: " + mailboxInfo.TasksUri);
            }
        }
        catch (Exception ex)
        {
            // Graceful error handling
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
