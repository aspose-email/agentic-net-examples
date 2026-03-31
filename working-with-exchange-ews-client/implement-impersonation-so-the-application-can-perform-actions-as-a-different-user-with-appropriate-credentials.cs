using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailImpersonationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip execution in CI environments
                string serviceUrl = "https://example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create EWS client using the verified factory method
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Impersonate another user (Primary SMTP address)
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "impersonated@example.com");

                    // List messages in the impersonated user's Inbox
                    ExchangeMessageInfoCollection messagesInfo = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messagesInfo != null && messagesInfo.Count > 0)
                    {
                        // Use the UniqueUri of the first message to fetch it
                        ExchangeMessageInfo firstInfo = messagesInfo[0];
                        using (MailMessage message = client.FetchMessage(firstInfo.UniqueUri))
                        {
                            Console.WriteLine("Subject: " + message.Subject);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No messages found in the impersonated mailbox.");
                    }

                    // Reset impersonation when done
                    client.ResetImpersonation();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
