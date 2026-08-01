using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Author: Aspose.Email example - filter messages by Message-ID header using Exchange WebDAV client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string folder = "Inbox"; // target folder
            string targetMessageId = "<unique-message-id@example.com>";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || targetMessageId.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Build a simple DASL query to match the Message-ID header
            string query = $"Message-ID='{targetMessageId}'";

            try
            {
                using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
                {
                    // List messages that match the query; returns ExchangeMessageInfoCollection
                    ExchangeMessageInfoCollection messages = client.ListMessages(folder, query);

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Message-ID: {info.MessageId}");
                        Console.WriteLine(new string('-', 40));
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
