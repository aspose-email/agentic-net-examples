using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define mailbox URI and credentials
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the asynchronous EWS client synchronously
            IAsyncEwsClient client = null;
            try
            {
                client = EWSClient.GetEwsClientAsync(mailboxUri, credentials).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client
            using (client)
            {
                try
                {
                    // Retrieve mailbox information as an example operation
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfoAsync().GetAwaiter().GetResult();
                    Console.WriteLine($"Mailbox display name: {mailboxInfo.MailboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving mailbox info: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
