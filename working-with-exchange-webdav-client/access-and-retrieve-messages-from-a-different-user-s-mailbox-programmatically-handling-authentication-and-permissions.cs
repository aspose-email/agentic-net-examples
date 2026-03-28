using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Server URL and delegate credentials
            string serverUrl = "https://exchange.example.com/exchange";
            string delegateUsername = "delegateUser";
            string delegatePassword = "password";

            // Target mailbox (different user)
            string targetMailbox = "targetUser@example.com";

            // Construct the mailbox URI for the target user
            string mailboxUri = $"{serverUrl}/{targetMailbox}";

            // Initialize the Exchange WebDAV client with delegate credentials
            using (ExchangeClient client = new ExchangeClient(mailboxUri, delegateUsername, delegatePassword))
            {
                // Retrieve messages from the Inbox folder of the target mailbox
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message using its unique URI
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
