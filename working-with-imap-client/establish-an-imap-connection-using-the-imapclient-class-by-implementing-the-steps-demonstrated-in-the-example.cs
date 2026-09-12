using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapConnectionSample
{
    // Author: Aspose.Email .NET example
    class Program
    {
        static void Main()
        {
            // IMAP server connection parameters (replace with real values)
            string host = "your_imap_host";
            int port = 993;
            string username = "your_username";
            string password = "your_password";

            // Guard: skip network call when placeholder credentials are detected
            bool placeholders = string.IsNullOrWhiteSpace(host) || host.Contains("your_") ||
                                string.IsNullOrWhiteSpace(username) || username.Contains("your_") ||
                                string.IsNullOrWhiteSpace(password) || password.Contains("your_");

            if (placeholders)
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Create and use the ImapClient inside a using block to ensure proper disposal
            try
            {
                // Initialize client with SSL implicit security
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve message summaries from the selected folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    Console.WriteLine($"Total messages in INBOX: {messages.Count}");

                    // Print subject of each message
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while connecting to IMAP server: {ex.Message}");
            }
        }
    }
}
