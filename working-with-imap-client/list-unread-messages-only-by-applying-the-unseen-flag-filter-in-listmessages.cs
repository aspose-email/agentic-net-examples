using Aspose.Email.Tools.Search;
using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – real credentials should be provided by the user.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder values are detected to avoid runtime failures.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            // Create and use the IMAP client within a using block to ensure proper disposal.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Build a query that selects messages which do NOT have the Read flag (i.e., unread).
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    MailQuery unreadQuery = queryBuilder.HasNoFlags(ImapMessageFlags.IsRead);

                    // Retrieve the list of unread messages.
                    ImapMessageInfoCollection unreadMessages = client.ListMessages(unreadQuery);

                    // Output basic information about each unread message.
                    foreach (ImapMessageInfo messageInfo in unreadMessages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From: {messageInfo.From}");
                        Console.WriteLine($"Date: {messageInfo.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    // Friendly error handling for client operations.
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
