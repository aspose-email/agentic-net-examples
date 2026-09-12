using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the IMAP client with connection settings
            ImapClient imapClient = new ImapClient(
                "imap.example.com",
                993,
                "user@example.com",
                "password",
                SecurityOptions.SSLImplicit);

            // Guard: skip network operations when placeholder credentials are used
            bool isPlaceholder = imapClient.Host.Contains("example.com") ||
                                 imapClient.Username.Contains("example.com") ||
                                 imapClient.Password == "password";

            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Use the client within a using block to ensure proper disposal
            using (imapClient)
            {
                // Build a complex search query:
                // Find messages from a specific sender, with a subject containing a keyword,
                // and received since a given date.
                ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                queryBuilder.From.Contains("alice@example.com");
                queryBuilder.Subject.Contains("Report");
                queryBuilder.InternalDate.Since(new DateTime(2023, 1, 1));

                // Retrieve the constructed MailQuery object
                MailQuery searchQuery = queryBuilder.GetQuery();

                // Execute the search on the IMAP server
                ImapMessageInfoCollection messages = imapClient.ListMessages(searchQuery);

                // Output basic information about each matching message
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
