using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace ImpersonationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";
                string impersonatedUser = "impersonated@example.com";

                // Guard against placeholder credentials to avoid real network calls
                if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping actual Exchange operations.");
                    return;
                }

                // Create the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Impersonate the alternate user
                        client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, impersonatedUser);

                        // List messages in the impersonated user's Inbox
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            Console.WriteLine("Subject: " + messageInfo.Subject);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during impersonation operation: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
