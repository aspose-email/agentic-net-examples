using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip execution
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Build a server‑side search query for keywords in the message body
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    queryBuilder.Body.Contains("Invoice");
                    queryBuilder.Body.Contains("Payment");
                    MailQuery query = queryBuilder.GetQuery();

                    // Execute the search
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    Console.WriteLine($"Found {messages.Count} message(s) containing the specified keywords.");

                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
