using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange client (WebDAV)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                client.Timeout = 100000;

                // List all messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages("Inbox");

                // Extract unique URIs for each message
                List<string> messageUris = new List<string>();
                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    // Use UniqueUri as the identifier for fetching
                    messageUris.Add(info.UniqueUri);
                }

                // Define batch size for retrieval
                int batchSize = 10;

                // Process messages in batches with retry logic
                for (int i = 0; i < messageUris.Count; i += batchSize)
                {
                    int count = Math.Min(batchSize, messageUris.Count - i);
                    List<string> batch = messageUris.GetRange(i, count);

                    const int maxRetries = 3;
                    int retry = 0;
                    bool success = false;

                    while (!success && retry <= maxRetries)
                    {
                        try
                        {
                            // Fetch the batch of messages
                            MailMessageCollection messages = client.FetchMessages(batch);

                            // Example processing: output subject lines
                            foreach (MailMessage msg in messages)
                            {
                                Console.WriteLine("Subject: " + msg.Subject);
                            }

                            success = true;
                        }
                        catch (Exception ex) when (IsTransient(ex))
                        {
                            retry++;
                            if (retry > maxRetries)
                            {
                                Console.Error.WriteLine($"Failed to fetch batch after {maxRetries} retries: {ex.Message}");
                            }
                            else
                            {
                                int delay = (int)Math.Pow(2, retry) * 1000;
                                Console.Error.WriteLine($"Transient error: {ex.Message}. Retrying in {delay} ms...");
                                Thread.Sleep(delay);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Determines whether an exception is considered transient
    private static bool IsTransient(Exception ex)
    {
        return ex is WebException || ex is IOException;
    }
}
