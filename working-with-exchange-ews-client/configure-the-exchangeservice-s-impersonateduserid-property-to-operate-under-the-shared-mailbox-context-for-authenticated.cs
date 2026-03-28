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
            // Credentials for the account that has permission to impersonate the shared mailbox
            NetworkCredential credentials = new NetworkCredential("username", "password", "domain");

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                try
                {
                    // Impersonate the shared mailbox using its primary SMTP address
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "sharedmailbox@example.com");

                    // List messages from the impersonated mailbox's Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Iterate through the messages and display their subjects
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Fetch the full message using its unique URI
                        using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                        {
                            Console.WriteLine(message.Subject);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors that occur during impersonation or message operations
                    Console.Error.WriteLine("Error during EWS operations: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors that occur while creating the client (e.g., authentication failures)
            Console.Error.WriteLine("Failed to connect to Exchange server: " + ex.Message);
            return;
        }
    }
}
