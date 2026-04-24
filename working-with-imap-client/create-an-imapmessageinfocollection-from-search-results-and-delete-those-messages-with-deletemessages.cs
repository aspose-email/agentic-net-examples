using Aspose.Email.Tools.Search;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapDeleteSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder IMAP server details – replace with real values.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                    return;
                }

                // Create and configure the IMAP client.
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        client.Username = username;
                        client.Password = password;

                        // Select the folder to operate on.
                        client.SelectFolder("INBOX");

                        // Build a search query (e.g., messages with "Sample" in the subject).
                        ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                        queryBuilder.Subject.Contains("Sample");
                        MailQuery query = queryBuilder.GetQuery();

                        // Retrieve matching messages.
                        ImapMessageInfoCollection messageInfoCollection = client.ListMessagesAsync(query, CancellationToken.None).Result;

                        if (messageInfoCollection != null && messageInfoCollection.Count > 0)
                        {
                            // Delete the retrieved messages and commit the deletions immediately.
                            client.DeleteMessages(messageInfoCollection, true);
                            Console.WriteLine($"{messageInfoCollection.Count} message(s) deleted.");
                        }
                        else
                        {
                            Console.WriteLine("No messages matched the search criteria.");
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
}
