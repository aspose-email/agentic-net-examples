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
            // Mailbox connection details
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
            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);
            try
            {
                // Add a header to request minimal data (optional, demonstrates lightweight request)
                client.AddHeader("Prefer", "outlook.body-content-type=\"text\"");

                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox (only retrieves message info, not full bodies)
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri);
                if (messageInfos == null || messageInfos.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Take the first message's unique URI
                string messageUri = messageInfos[0].UniqueUri;

                // Fetch the message (full MailMessage object)
                MailMessage mailMessage = client.FetchMessage(messageUri);

                // Output only the headers
                foreach (string headerKey in mailMessage.Headers.AllKeys)
                {
                    Console.WriteLine($"{headerKey}: {mailMessage.Headers[headerKey]}");
                }
            }
            finally
            {
                client.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
