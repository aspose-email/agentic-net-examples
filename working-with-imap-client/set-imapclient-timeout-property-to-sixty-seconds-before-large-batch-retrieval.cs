using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values when running against a real server.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder values are detected to avoid unwanted network calls.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                return;
            }

            // Create and configure the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Set timeout to 60 seconds (60000 milliseconds) before performing batch operations.
                    client.Timeout = 60000;

                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve a large batch of messages (e.g., up to 1000 messages).
                    ImapMessageInfoCollection messages = client.ListMessages(1000);

                    Console.WriteLine($"Retrieved {messages.Count} messages from INBOX.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
