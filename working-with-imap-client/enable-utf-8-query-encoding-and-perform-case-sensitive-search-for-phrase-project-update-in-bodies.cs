using System;
using System.Text;
using System.Threading;
using Aspose.Email;
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
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client within a using block
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Enable UTF-8 encoding for the query builder
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder(Encoding.UTF8);

                    // Build a case‑sensitive search for the exact phrase in the message body
                    MailQuery query = queryBuilder.Body.Contains("Project Update");

                    // Execute the search synchronously (awaiting the task result)
                    ImapMessageInfoCollection messages = client.ListMessagesAsync(query, CancellationToken.None).Result;

                    // Output found messages
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"UID: {messageInfo.UniqueId}, Subject: {messageInfo.Subject}");
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
