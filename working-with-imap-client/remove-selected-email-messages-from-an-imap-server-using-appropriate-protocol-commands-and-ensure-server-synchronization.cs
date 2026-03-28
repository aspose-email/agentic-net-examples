using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // IMAP server connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create and connect the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the folder to operate on
                        client.SelectFolder("INBOX");

                        // Build a query to identify messages to delete (e.g., subject contains "DeleteMe")
                        MailQueryBuilder builder = new MailQueryBuilder();
                        builder.Subject.Contains("DeleteMe");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages matching the query
                        IEnumerable<ImapMessageInfo> messagesToDelete = client.ListMessages(query);

                        // Delete the messages and commit the deletions
                        if (messagesToDelete != null)
                        {
                            client.DeleteMessages(messagesToDelete, true);
                            Console.WriteLine("Selected messages have been deleted and changes committed.");
                        }
                        else
                        {
                            Console.WriteLine("No messages matched the deletion criteria.");
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
}
