using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Placeholder credentials – replace with real values for actual execution.
        string mailboxUri = "https://example.com/EWS/Exchange.asmx";
        string username = "username";
        string password = "password";

        // Guard: skip network call when placeholders are detected.
        if (mailboxUri.Contains("example") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
            return;
        }

        try
        {
            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Enable tracing by specifying a log file. This activates runtime tracing.
                client.LogFileName = "ews_trace.log";

                // Example operation: retrieve mailbox information.
                var mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine("Mailbox URI: " + mailboxInfo.MailboxUri);
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred while accessing Exchange: " + ex.Message);
        }
    }
}
