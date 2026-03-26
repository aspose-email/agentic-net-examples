using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the WebDAV endpoint and credentials for the Exchange mailbox
            string exchangeUri = "https://exchange.example.com/exchange/user@example.com/";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the Exchange client inside a using block to ensure proper disposal
            using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(exchangeUri, credentials))
            {
                // Retrieve the collection of messages from the Inbox folder
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);

                // Iterate through each message info and process the corresponding MailMessage
                foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo messageInfo in messageInfos)
                {
                    // Fetch the full MailMessage using the unique URI of the message
                    using (Aspose.Email.MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueUri))
                    {
                        // Example processing: output subject, sender, and body to the console
                        Console.WriteLine("Subject: " + mailMessage.Subject);
                        if (mailMessage.From != null)
                        {
                            Console.WriteLine("From: " + mailMessage.From.DisplayName + " <" + mailMessage.From.Address + ">");
                        }
                        Console.WriteLine("Body: " + mailMessage.Body);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error output stream
            Console.Error.WriteLine("Error: " + ex.Message);
            return;
        }
    }
}