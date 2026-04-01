using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Example sequence numbers to fetch; adjust as needed
                    List<int> sequenceNumbers = new List<int> { 1, 2 };

                    // Asynchronously fetch messages
                    Task<IList<MailMessage>> fetchTask = client.FetchMessagesAsync(sequenceNumbers);
                    IList<MailMessage> messages = fetchTask.GetAwaiter().GetResult();

                    // Process fetched messages
                    foreach (MailMessage message in messages)
                    {
                        using (message)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during fetch: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
