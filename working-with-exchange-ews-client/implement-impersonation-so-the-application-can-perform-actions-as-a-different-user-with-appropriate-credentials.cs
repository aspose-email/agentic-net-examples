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
            // Define connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "admin@example.com";
            string password = "adminPassword";
            string impersonatedUser = "user@example.com";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Impersonate the target user
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, impersonatedUser);

                    // Retrieve mailbox information for the impersonated user
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation error: {ex.Message}");
                }
                finally
                {
                    // Reset impersonation if needed
                    client.ResetImpersonation();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
