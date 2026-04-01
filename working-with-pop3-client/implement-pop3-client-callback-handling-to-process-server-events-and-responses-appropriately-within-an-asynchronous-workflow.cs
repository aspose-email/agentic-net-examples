using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder POP3 server details
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping POP3 operations.");
                return;
            }

            // Initialize POP3 client with placeholder credentials
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Subscribe to connection event
                client.OnConnect += (sender, e) => Console.WriteLine("Connected to POP3 server.");

                // Asynchronously list messages
                Pop3MessageInfoCollection messages = await client.ListMessagesAsync();

                Console.WriteLine($"Total messages: {messages.Count}");

                // Iterate through messages and display basic info
                foreach (var info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}, Size: {info.Size} bytes");
                }

                // Fetch the first message asynchronously as a demonstration
                if (messages.Count > 0)
                {
                    var firstInfo = messages[0];
                    var message = await client.FetchMessageAsync(firstInfo.SequenceNumber);
                    Console.WriteLine($"Fetched message subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
