using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System.Net;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – replace with real values for actual use.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string sharedMailbox = "shared@example.com";

                // Guard against executing real network calls when placeholders are present.
                if (mailboxUri.Contains("example.com") ||
                    username.Contains("example.com") ||
                    password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected – skipping actual Exchange connection.");
                    return;
                }

                // Create the EWS client using the factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Impersonate the shared mailbox using its primary SMTP address.
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, sharedMailbox);

                    // Example operation under impersonation – list messages in the Inbox.
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    Console.WriteLine($"Impersonated mailbox '{sharedMailbox}' contains {messages.Count} messages in the Inbox.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
