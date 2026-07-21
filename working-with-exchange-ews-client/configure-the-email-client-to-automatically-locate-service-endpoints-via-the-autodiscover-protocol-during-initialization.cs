using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Placeholder credentials – replace with real values when running against a live server
        string email = "your-email@example.com";
        string password = "your-password";

        // Guard: skip network operations if placeholders are detected
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            email.Contains("example.com") ||
            password.Contains("your-"))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS client initialization.");
            return;
        }

        try
        {
            // Create network credentials
            NetworkCredential credentials = new NetworkCredential(email, password);

            // AutoDiscover the EWS endpoint based on the email address
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(email, credentials))
            {
                // Sample operation: retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error initializing EWS client: " + ex.Message);
        }
    }
}
