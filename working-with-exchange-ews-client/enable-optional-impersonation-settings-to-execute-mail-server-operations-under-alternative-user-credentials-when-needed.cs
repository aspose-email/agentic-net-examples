using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with placeholder credentials
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                try
                {
                    // Enable impersonation using primary SMTP address
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "impersonated@example.com");

                    // List messages in the impersonated user's Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messages != null && messages.Count > 0)
                    {
                        // Fetch the first message using its UniqueUri
                        MailMessage firstMessage = client.FetchMessage(messages[0].UniqueUri);
                        Console.WriteLine("Subject: " + firstMessage.Subject);
                    }
                    else
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                    }

                    // Reset impersonation after operation
                    client.ResetImpersonation();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during EWS operations: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
        }
    }
}
