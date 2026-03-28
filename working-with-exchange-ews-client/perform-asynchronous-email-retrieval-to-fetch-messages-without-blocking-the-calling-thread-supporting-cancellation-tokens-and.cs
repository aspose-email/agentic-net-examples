using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

public class Program
{
    public static async Task Main()
    {
        try
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                // Placeholder credentials – replace with real values.
                NetworkCredential credential = new NetworkCredential("username", "password");
                string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";

                // Create the asynchronous EWS client.
                using (IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(
                    serviceUrl, credential, null, cts.Token))
                {
                    // Obtain mailbox information (optional, used to get the Inbox URI).
                    ExchangeMailboxInfo mailboxInfo = await client.GetMailboxInfoAsync(null, cts.Token);
                    string inboxFolder = mailboxInfo.InboxUri;

                    // List up to 10 messages from the Inbox.
                    ExchangeMessageInfoCollection messageInfos = await client.ListMessagesAsync(
                        folder: inboxFolder,
                        mailbox: null,
                        maxNumberOfMessages: 10,
                        query: null,
                        recursive: false,
                        extendedProperties: null,
                        cancellationToken: cts.Token);

                    Console.WriteLine($"Found {messageInfos.Count} messages in Inbox.");

                    // Collect the unique URIs of the messages.
                    IEnumerable<string> uris = messageInfos.Select(info => info.UniqueUri);

                    // Fetch the full messages asynchronously.
                    MailMessageCollection messages = await client.FetchMessagesAsync(
                        uris,
                        extendedProperties: null,
                        cancellationToken: cts.Token);

                    // Process the fetched messages.
                    foreach (MailMessage msg in messages)
                    {
                        Console.WriteLine($"Subject: {msg.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
