using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;

namespace AsyncEmailRetrieval
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(2));
                CancellationToken token = cts.Token;

                // Exchange EWS service URL and credentials (placeholders)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string domain = ""; // optional, leave empty if not needed

                // Detect placeholder credentials and skip external calls
                if (serviceUrl.Contains("example.com") ||
                    username.Contains("example.com") ||
                    password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the Exchange client (implements IEWSClient)
                IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password, domain);

                try
                {
                    // List messages in the Inbox folder asynchronously
                    var messageInfos = await Task.Run(() => client.ListMessages("Inbox"), token);

                    if (messageInfos == null || messageInfos.Count == 0)
                    {
                        Console.WriteLine("No messages found in Inbox.");
                        return;
                    }

                    var messages = new List<MailMessage>();

                    // Fetch each message asynchronously, supporting cancellation
                    foreach (var info in messageInfos)
                    {
                        token.ThrowIfCancellationRequested();

                        // FetchMessage may be a synchronous call; wrap it in Task.Run
                        var message = await Task.Run(() => client.FetchMessage(info.UniqueUri), token);
                        messages.Add(message);
                    }

                    // Process retrieved messages
                    foreach (var message in messages)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Additional processing (e.g., saving attachments) can be added here
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.Error.WriteLine("Operation was canceled.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
