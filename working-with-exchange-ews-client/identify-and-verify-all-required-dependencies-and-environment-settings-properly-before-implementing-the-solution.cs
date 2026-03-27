using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the EWS client within a using block for proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // List messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Output basic metadata for each message
                foreach (ExchangeMessageInfo info in messages)
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
            // Friendly error handling
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
