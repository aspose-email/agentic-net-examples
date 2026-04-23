using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI.
            if (host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                return;
            }

            // Connect to the IMAP server.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (lightweight check).
                    if (!client.ValidateCredentials())
                    {
                        Console.Error.WriteLine("Authentication failed.");
                        return;
                    }

                    // Build a case‑insensitive subject search for the keyword "invoice".
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    // The Subject.Contains method performs a case‑insensitive match.
                    queryBuilder.Subject.Contains("invoice");

                    // Retrieve messages that match the query.
                    ImapMessageInfoCollection messages = client.ListMessages(queryBuilder.GetQuery());

                    Console.WriteLine($"Found {messages.Count} message(s) with \"invoice\" in the subject:");

                    foreach (ImapMessageInfo info in messages)
                    {
                        // Fetch the full message to read its subject (optional – subject is also in info).
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        Console.WriteLine($"- Subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
