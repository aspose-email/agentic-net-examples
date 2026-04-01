using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip actual execution in CI environments.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                if (mailboxUri.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create the EWS client using the verified factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Enable impersonation (optional). Replace with the desired impersonated address.
                        client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "impersonated@example.com");

                        // Example operation: list messages in the Inbox folder.
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine(info.UniqueUri);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation error: {ex.Message}");
                    }
                    finally
                    {
                        // Reset impersonation to avoid side effects.
                        client.ResetImpersonation();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled error: {ex.Message}");
            }
        }
    }
}
