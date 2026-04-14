using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            const int maxRetries = 3;
            const int delayMilliseconds = 2000;
            int attempt = 0;
            bool connected = false;

            while (attempt < maxRetries && !connected)
            {
                attempt++;
                try
                {
                    // Create EWS client (implements IDisposable)
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Simple operation to verify connection – fetch mailbox info
                        ExchangeMailboxInfo info = client.MailboxInfo;
                        Console.WriteLine($"Connected. Mailbox display name: {info.MailboxUri}");
                        connected = true;
                    }
                }
                catch (ExchangeException ex)
                {
                    Console.Error.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                    if (attempt >= maxRetries)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Exiting.");
                        return;
                    }
                    Thread.Sleep(delayMilliseconds);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error on attempt {attempt}: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
