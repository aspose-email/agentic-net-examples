using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace with actual server URI and credentials)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Obtain the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List basic information about messages in the Inbox
                ExchangeMessageInfoCollection messagesInfo = client.ListMessages(inboxUri);

                foreach (var info in messagesInfo)
                {
                    // Fetch the message (headers are included in the MailMessage object)
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        foreach (var headerKey in message.Headers.Keys)
                        {
                            Console.WriteLine($"{headerKey}: {message.Headers[headerKey]}");
                        }
                        Console.WriteLine(new string('-', 40));
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
