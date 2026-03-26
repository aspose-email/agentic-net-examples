using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Create and connect to the Exchange server
            using (IEWSClient client = EWSClient.GetEWSClient("https://example.com/EWS/Exchange.asmx", new NetworkCredential("username", "password")))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve message metadata from the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Process each message's metadata
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + (info.From != null ? info.From.ToString() : string.Empty));
                    Console.WriteLine("Date: " + info.Date);
                    Console.WriteLine("To: " + (info.To != null ? info.To.ToString() : string.Empty));
                    Console.WriteLine("Size: " + info.Size);
                    Console.WriteLine("Has Attachments: " + info.HasAttachments);
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