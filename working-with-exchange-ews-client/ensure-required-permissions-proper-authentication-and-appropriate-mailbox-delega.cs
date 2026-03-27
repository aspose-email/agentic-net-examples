using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // EWS service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";

            // Credentials for authentication
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client via the factory method
            using (Aspose.Email.Clients.Exchange.WebService.IEWSClient client = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Example: list messages in the primary mailbox's Inbox
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + info.From);
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}