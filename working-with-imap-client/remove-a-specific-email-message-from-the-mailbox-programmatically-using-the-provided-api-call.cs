using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – skip real network call in CI environments
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("username") || password.Contains("password"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping delete operation.");
                return;
            }

            // Initialize the IMAP client (will be disposed automatically)
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Build a query to locate the specific message (e.g., by subject)
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Specific Subject");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages matching the criteria were found.");
                        return;
                    }

                    // Delete the found messages and commit the deletions immediately
                    client.DeleteMessages(messages, true);
                    Console.WriteLine($"{messages.Count} message(s) deleted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle client-specific errors (connection, authentication, etc.)
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
