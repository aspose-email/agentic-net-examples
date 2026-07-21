using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

// Author: Aspose.Email .NET example

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string sharedMailbox = "shared@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || sharedMailbox.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Credentials for the primary user
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Impersonate the shared mailbox
                client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, sharedMailbox);

                // Retrieve mailbox folder URIs
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // List messages in the shared mailbox Inbox
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(mailboxInfo.InboxUri);

                Console.WriteLine($"Found {messageInfos.Count} messages in the shared mailbox Inbox.");

                foreach (ExchangeMessageInfo messageInfo in messageInfos)
                {
                    // Fetch the full message
                    MailMessage message = client.FetchMessage(messageInfo.UniqueUri);
                    Console.WriteLine($"Subject: {message.Subject}");
                }

                // Reset impersonation after operation
                client.ResetImpersonation();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
