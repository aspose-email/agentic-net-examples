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
            // Initialize IMAP client with connection parameters.
            // Replace placeholder values with actual server details.
            using (ImapClient client = new ImapClient("imap.example.com", "username", "password", SecurityOptions.Auto))
            {
                // Select the folder to search.
                client.SelectFolder("INBOX");

                // Build a complex search query.
                ImapQueryBuilder builder = new ImapQueryBuilder();
                // Find messages where the subject contains "Report".
                builder.Subject.Contains("Report");
                // Find messages from a specific domain.
                builder.From.Contains("@example.com");
                // Exclude messages marked as deleted.
                builder.HasNoFlags(ImapMessageFlags.Deleted);

                // Retrieve the constructed MailQuery.
                MailQuery query = builder.GetQuery();

                // Execute the search and retrieve matching messages.
                ImapMessageInfoCollection messages = client.ListMessages(query);

                // Output basic information about each matched message.
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors.
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
