using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

namespace ImapSample
{
    class Program
    {
        static void Main()
        {
            // Placeholder credentials – replace with real values when needed.
            string host = "imap.example.com";
            int port = 993;
            string username = "your_username";
            string password = "your_password";

            // Guard: skip network calls when placeholder credentials are used.
            if (username.StartsWith("your_") || password.StartsWith("your_"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping network operations.");
                return;
            }

            // Create the IMAP client. No network activity occurs until a method is called.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve message summaries.
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    foreach (var info in messageInfos)
                    {
                        // Fetch the full message using its unique identifier.
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
