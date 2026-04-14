using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main()
    {
        try
        {
            // Mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create asynchronous EWS client
            IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            try
            {
                // List of message URIs to fetch
                List<string> messageUris = new List<string>
                {
                    "https://exchange.example.com/EWS/Message1",
                    "https://exchange.example.com/EWS/Message2"
                };

                // Asynchronously fetch messages
                MailMessageCollection messages = await client.FetchMessagesAsync(messageUris);

                // Process fetched messages
                foreach (MailMessage message in messages)
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation error: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose the client
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
