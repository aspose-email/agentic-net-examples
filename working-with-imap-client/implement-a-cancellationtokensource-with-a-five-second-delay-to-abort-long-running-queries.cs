using Aspose.Email.Tools.Search;
using Aspose.Email.Clients;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected.
            if (host.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Simple validation: select the INBOX folder.
                client.SelectFolder("INBOX");

                // Create a cancellation token that aborts after 5 seconds.
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    // Retrieve all messages (null query) with cancellation support.
                    var messages = client.ListMessagesAsync((MailQuery)null, token: cts.Token)
                                         .GetAwaiter()
                                         .GetResult();

                    Console.WriteLine($"Retrieved {messages.Count} messages.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
