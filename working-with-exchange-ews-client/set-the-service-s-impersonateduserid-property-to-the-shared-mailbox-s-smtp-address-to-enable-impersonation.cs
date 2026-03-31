using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailImpersonationExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder values – replace with real credentials when running against a real server.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string sharedMailboxSmtp = "shared@example.com";

                // Guard against executing live network calls with placeholder data.
                if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                    return;
                }

                // Create the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Impersonate the shared mailbox using its SMTP address.
                        client.ImpersonateUser(ItemChoice.SmtpAddress, sharedMailboxSmtp);
                        Console.WriteLine($"Impersonation set to {sharedMailboxSmtp}.");

                        // Additional EWS operations can be performed here while impersonating.
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during impersonation: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
