using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and user credentials
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            string domain = ""; // optional domain

            NetworkCredential credentials = new NetworkCredential(username, password, domain);

            // Create the EWS client
            IEWSClient ewsClient;
            try
            {
                ewsClient = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (ewsClient)
            {
                // Impersonate the shared mailbox (requires delegation rights)
                try
                {
                    ewsClient.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "shared@example.com");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Impersonation failed: {ex.Message}");
                    return;
                }

                // Access the Inbox of the shared mailbox
                try
                {
                    ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    ExchangeMessageInfoCollection messages = ewsClient.ListMessages(inboxUri);
                    Console.WriteLine($"Shared mailbox inbox contains {messages.Count} messages.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing shared mailbox: {ex.Message}");
                }
                finally
                {
                    // Reset impersonation to original user
                    try { ewsClient.ResetImpersonation(); } catch { }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
