using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            // Server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Initialize IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select INBOX folder
                imapClient.SelectFolder("INBOX");

                // Build query to find messages to delete (e.g., subject contains "DeleteMe")
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("DeleteMe");
                MailQuery query = builder.GetQuery();

                // Retrieve messages matching the query
                IList<ImapMessageInfo> messagesToDelete = imapClient.ListMessages(query);

                if (messagesToDelete != null && messagesToDelete.Count > 0)
                {
                    // Delete messages and commit deletions
                    imapClient.DeleteMessages(messagesToDelete, true);
                    Console.WriteLine($"{messagesToDelete.Count} message(s) deleted and changes committed.");
                }
                else
                {
                    Console.WriteLine("No messages matched the deletion criteria.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}