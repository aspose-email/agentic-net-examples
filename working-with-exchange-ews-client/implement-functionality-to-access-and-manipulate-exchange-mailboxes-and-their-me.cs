using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Placeholder credentials and service URL
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client safely
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                    {
                        // List messages in the Inbox folder
                        try
                        {
                            ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                            foreach (ExchangeMessageInfo info in messages)
                            {
                                Console.WriteLine("Subject: " + info.Subject);
                                Console.WriteLine("From: " + (info.From != null ? info.From.DisplayName : "Unknown"));
                                Console.WriteLine("Received: " + info.Date);
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("Error listing messages: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error creating EWS client: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}