using System;
using System.Diagnostics;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration - replace with real values
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Prepare a test email
            MailMessage testMessage = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Performance Test",
                "This is a test message for latency measurement."
            );

            // Measure round‑trip latency
            Stopwatch stopwatch = new Stopwatch();

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Send the message
                stopwatch.Start();
                client.Send(testMessage);
                stopwatch.Stop();
                Console.WriteLine($"Send latency: {stopwatch.ElapsedMilliseconds} ms");

                // Allow server a moment to process the message
                System.Threading.Thread.Sleep(2000);

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);
                if (inboxMessages == null || inboxMessages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Find the message we just sent by subject
                string targetUri = null;
                foreach (ExchangeMessageInfo info in inboxMessages)
                {
                    if (info.Subject == testMessage.Subject)
                    {
                        targetUri = info.UniqueUri;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(targetUri))
                {
                    Console.Error.WriteLine("Sent message not found in the Inbox.");
                    return;
                }

                // Fetch the message and measure fetch latency
                stopwatch.Restart();
                MailMessage fetchedMessage = client.FetchMessage(targetUri);
                stopwatch.Stop();
                Console.WriteLine($"Fetch latency: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
