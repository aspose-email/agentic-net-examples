using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace EmailMetadataSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service URL and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credential = new NetworkCredential("username", "password");

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
                {
                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Iterate through each message and output metadata fields
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("From: " + (info.From != null ? info.From.ToString() : string.Empty));
                        Console.WriteLine("Date: " + info.Date);
                        Console.WriteLine("Sender: " + (info.Sender != null ? info.Sender.ToString() : string.Empty));
                        Console.WriteLine("Size: " + info.Size);
                        Console.WriteLine("Message ID: " + info.MessageId);
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
}
