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
            // Set up credentials (replace with real values)
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                // Get the URI of the Inbox folder
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve messages from the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Output basic information about each message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
