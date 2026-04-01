using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace RemoveImapMessagesSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";

                // Skip execution when placeholders are detected to avoid external calls during CI.
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and connect the IMAP client.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Select the target folder (e.g., INBOX).
                    client.SelectFolder("INBOX");

                    // Build a query to locate the messages that should be removed.
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("DeleteMe"); // Adjust criteria as needed.
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query.
                    ImapMessageInfoCollection messagesToDelete = client.ListMessages(query);

                    if (messagesToDelete == null || messagesToDelete.Count == 0)
                    {
                        Console.WriteLine("No messages matched the deletion criteria.");
                        return;
                    }

                    // Delete the selected messages and commit the deletions immediately.
                    client.DeleteMessages(messagesToDelete, true); // 'true' commits the deletions.

                    Console.WriteLine($"{messagesToDelete.Count} message(s) deleted and changes committed.");
                }
            }
            catch (Exception ex)
            {
                // Gracefully report any errors.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
