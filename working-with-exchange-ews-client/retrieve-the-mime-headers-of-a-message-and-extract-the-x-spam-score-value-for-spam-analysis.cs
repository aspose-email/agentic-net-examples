using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

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


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Fetch the first message
                string messageUri = messages[0].UniqueUri;
                MailMessage mail = client.FetchMessage(messageUri);

                // Extract the X-Spam-Score header
                string spamScore = mail.Headers["X-Spam-Score"];
                if (!string.IsNullOrEmpty(spamScore))
                {
                    Console.WriteLine($"X-Spam-Score: {spamScore}");
                }
                else
                {
                    Console.WriteLine("X-Spam-Score header not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
