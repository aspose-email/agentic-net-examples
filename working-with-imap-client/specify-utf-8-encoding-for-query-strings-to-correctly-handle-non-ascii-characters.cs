using Aspose.Email.Tools.Search;
using System;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Connection parameters (replace with real values)
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder (auto‑connects)
                    client.SelectFolder("INBOX");

                    // Build a UTF‑8 encoded query (e.g., search for a non‑ASCII subject)
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder(Encoding.UTF8);
                    MailQuery query = queryBuilder.Subject.Contains("こんにちは");

                    // Execute the search
                    ImapMessageInfoCollection messages = client.ListMessages(query);
                    Console.WriteLine($"Found {messages.Count} message(s) matching the UTF‑8 query.");

                    // Fetch and display subjects of the matched messages
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        MailMessage message = client.FetchMessage(messageInfo.UniqueId);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
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
