using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://your-ews-server/EWS/Exchange.asmx";
            string username = "your_username@example.com";
            string password = "your_password";
            string sharedMailboxSmtp = "sharedmailbox@example.com";

            // Guard against executing network calls with placeholder credentials.
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                username.Contains("your") ||
                password.Contains("your"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS impersonation example.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Enable impersonation of the shared mailbox.
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, sharedMailboxSmtp);

                    // Example operation: retrieve mailbox information.
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine("Impersonated mailbox display name: " + mailboxInfo.MailboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                }
                finally
                {
                    // Reset impersonation when done.
                    client.ResetImpersonation();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
