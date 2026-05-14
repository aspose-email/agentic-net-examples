using System;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            string host = "pop3.example.com";
            int port = 110;
            string username = "user";
            string password = "pass";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 host detected. Skipping execution.");
                return;
            }

            // Create POP3 client and enable diagnostic logging
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                client.EnableLogger = true;
                client.LogFileName = "pop3log.txt";

                // Validate connection credentials
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection/validation failed: {ex.Message}");
                    return;
                }

                // Measure GetMessageCount execution time
                Stopwatch stopwatch = Stopwatch.StartNew();
                int messageCount = client.GetMessageCount();
                stopwatch.Stop();
                Console.WriteLine($"GetMessageCount executed in {stopwatch.ElapsedMilliseconds} ms. Count: {messageCount}");

                // Measure ListMessages execution time
                stopwatch.Restart();
                Pop3MessageInfoCollection messages = client.ListMessages();
                stopwatch.Stop();
                Console.WriteLine($"ListMessages executed in {stopwatch.ElapsedMilliseconds} ms. Retrieved {messages.Count} messages.");

                // If there are messages, fetch the first one and measure the time
                if (messages.Count > 0)
                {
                    int sequenceNumber = messages[0].SequenceNumber;
                    stopwatch.Restart();
                    using (MailMessage fetchedMessage = client.FetchMessage(sequenceNumber))
                    {
                        stopwatch.Stop();
                        Console.WriteLine($"FetchMessage(seq={sequenceNumber}) executed in {stopwatch.ElapsedMilliseconds} ms. Subject: {fetchedMessage.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
