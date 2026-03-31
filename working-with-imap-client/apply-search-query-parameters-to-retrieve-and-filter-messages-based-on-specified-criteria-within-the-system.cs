using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP server detected. Skipping connection.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the folder to search
                    client.SelectFolder("INBOX");

                    // Build the search query (e.g., subject contains "Report" and from contains "alice@example.com")
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Report");
                    builder.From.Contains("alice@example.com");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Process each matching message
                    foreach (ImapMessageInfo info in messages)
                    {
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine();
                        }
                    }
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
