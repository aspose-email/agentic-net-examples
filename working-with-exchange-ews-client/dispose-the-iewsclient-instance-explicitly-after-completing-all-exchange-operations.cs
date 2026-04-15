using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define mailbox URI and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");

                IEWSClient client = null;
                try
                {
                    // Create the EWS client
                    client = EWSClient.GetEWSClient(mailboxUri, credentials);

                    // Perform an operation – retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine("Mailbox display name: " + mailboxInfo.MailboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                    return;
                }
                finally
                {
                    // Explicitly dispose the client
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
