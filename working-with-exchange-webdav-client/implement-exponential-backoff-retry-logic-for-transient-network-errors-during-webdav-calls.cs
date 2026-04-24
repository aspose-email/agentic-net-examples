using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (mailboxUri.Contains("example") || username.Contains("example") || password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create the WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                client.Timeout = 100000; // 100 seconds

                const int maxRetries = 5;
                int attempt = 0;
                int delayMs = 1000; // initial delay: 1 second
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = null;

                while (attempt < maxRetries)
                {
                    try
                    {
                        // Attempt to list messages from the Inbox folder.
                        messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        break; // Success – exit retry loop.
                    }
                    catch (ExchangeException ex) // Transient network/communication errors.
                    {
                        attempt++;
                        if (attempt >= maxRetries)
                        {
                            Console.Error.WriteLine($"Operation failed after {attempt} attempts: {ex.Message}");
                            return;
                        }

                        Console.Error.WriteLine($"Transient error ({ex.Message}). Retrying in {delayMs} ms (attempt {attempt}/{maxRetries})...");
                        Thread.Sleep(delayMs);
                        delayMs *= 2; // Exponential backoff.
                    }
                }

                if (messages != null)
                {
                    foreach (var info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
