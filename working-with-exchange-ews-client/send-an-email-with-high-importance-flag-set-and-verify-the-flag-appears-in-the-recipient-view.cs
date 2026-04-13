using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with actual server details
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
                // Compose a mail message with high importance
                MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "High Importance Test",
                    "This is a high importance email."
                );
                message.Priority = MailPriority.High; // Set high importance flag

                // Send the message
                client.Send(message);

                // Verify the importance flag in the Sent Items folder
                string sentFolderUri = client.MailboxInfo.SentItemsUri;
                ExchangeMessageInfoCollection sentMessages = client.ListMessages(sentFolderUri);
                foreach (ExchangeMessageInfo info in sentMessages)
                {
                    // Fetch the full message to inspect its priority
                    MailMessage fetched = client.FetchMessage(info.UniqueUri);
                    if (fetched.Subject == message.Subject)
                    {
                        Console.WriteLine("Sent message priority: " + fetched.Priority);
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
