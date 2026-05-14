using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create and configure the POP3 client.
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Enable internal logging (optional).
                    client.EnableLogger = true;
                    client.LogFileName = "pop3log.txt";

                    // List messages on the server.
                    Pop3MessageInfoCollection messageInfos = await client.ListMessagesAsync();

                    foreach (Pop3MessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full message.
                        using (MailMessage message = await client.FetchMessageAsync(messageInfo.SequenceNumber))
                        {
                            // Log sequence number and the message's original date.
                            Console.WriteLine($"Fetched message Seq:{messageInfo.SequenceNumber} Date:{messageInfo.Date}");
                        }
                    }
                }
                catch (Pop3Exception popEx)
                {
                    Console.Error.WriteLine($"POP3 error: {popEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
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
