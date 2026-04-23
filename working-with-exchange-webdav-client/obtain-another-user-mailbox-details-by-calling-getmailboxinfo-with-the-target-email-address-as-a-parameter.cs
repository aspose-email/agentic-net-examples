using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string targetEmail = "target@example.com";

            // Skip execution when placeholder credentials are detected.
            if (mailboxUri.Contains("example") || username.Contains("example") || password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the Exchange WebDav client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Obtain mailbox information for the specified user.
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo(targetEmail);

                    // Display selected mailbox URIs.
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error retrieving mailbox info: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
