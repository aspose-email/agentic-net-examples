using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange WebDAV client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (ExchangeClient client = new ExchangeClient(serviceUrl, credentials))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages("Inbox");
                foreach (ExchangeMessageInfo messageInfo in messageInfos)
                {
                    Console.WriteLine($"Message URI: {messageInfo.UniqueUri}");

                    // Fetch the full message content
                    using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Body: {message.Body}");
                        Console.WriteLine(new string('-', 40));
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
