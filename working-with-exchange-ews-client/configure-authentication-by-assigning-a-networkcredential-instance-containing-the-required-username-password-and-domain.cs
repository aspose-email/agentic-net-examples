using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Configure credentials
                NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

                // Initialize EWS client with credentials
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
                    {
                        try
                        {
                            // Retrieve mailbox information
                            ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                            // Output some mailbox URIs
                            Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                            Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("Operation error: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Connection error: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
