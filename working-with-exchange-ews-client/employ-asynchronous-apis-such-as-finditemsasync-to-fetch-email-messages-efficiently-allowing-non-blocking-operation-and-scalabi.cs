using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailAsyncSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder credentials detection – skip execution in CI environments
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Initialize asynchronous EWS client
                IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
                try
                {
                    // Asynchronously list messages in the Inbox folder without a query (fetch all)
                    ExchangeMessageInfoCollection messages = await client.ListMessagesAsync(
                        folder: "Inbox",
                        mailbox: null,
                        maxNumberOfMessages: 0,
                        query: null,
                        recursive: false,
                        extendedProperties: null,
                        cancellationToken: CancellationToken.None);

                    // Output basic information for each message
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
                finally
                {
                    // Ensure client resources are released
                    if (client is IDisposable disposableClient)
                    {
                        disposableClient.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
