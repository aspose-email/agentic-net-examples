using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Connection parameters (replace with actual values)
                string mailboxUri = "https://exchange.example.com/exchange/user@example.com/";
                string username = "user@example.com";
                string password = "password";

                // Create network credentials
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(username, password);

                // Initialize Exchange WebDAV client
                using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(mailboxUri, credentials))
                {
                    // Retrieve messages from the Inbox folder
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                    // Iterate through each message info
                    foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                    {
                        // Fetch the full message to access the plain‑text body
                        using (Aspose.Email.MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine("Subject: " + info.Subject);
                            Console.WriteLine("From: " + info.From);
                            Console.WriteLine("Body: " + message.Body);
                            Console.WriteLine(new string('-', 40));
                        }
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