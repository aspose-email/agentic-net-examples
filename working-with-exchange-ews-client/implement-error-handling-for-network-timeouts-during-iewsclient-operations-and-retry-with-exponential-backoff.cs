using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsRetryExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Optional: adjust timeout (milliseconds)
                    client.Timeout = 30000; // 30 seconds

                    const int maxRetries = 3;
                    int attempt = 0;
                    bool operationSucceeded = false;

                    while (attempt < maxRetries && !operationSucceeded)
                    {
                        try
                        {
                            // Attempt to list messages in the Inbox folder
                            ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                            // Process retrieved messages
                            foreach (ExchangeMessageInfo messageInfo in messages)
                            {
                                Console.WriteLine($"Subject: {messageInfo.Subject}");
                            }

                            operationSucceeded = true; // Success, exit retry loop
                        }
                        catch (ExchangeException ex)
                        {
                            // Handle network timeout or other Exchange errors
                            attempt++;
                            Console.Error.WriteLine($"Attempt {attempt} failed: {ex.Message}");

                            if (attempt >= maxRetries)
                            {
                                Console.Error.WriteLine("Maximum retry attempts reached. Operation aborted.");
                                break;
                            }

                            // Exponential backoff before next retry
                            int backoffDelayMs = (int)Math.Pow(2, attempt) * 1000; // 2^attempt seconds
                            Thread.Sleep(backoffDelayMs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
