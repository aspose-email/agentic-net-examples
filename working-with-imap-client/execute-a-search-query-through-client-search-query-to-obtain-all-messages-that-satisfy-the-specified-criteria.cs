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
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Initialize and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception folderEx)
                {
                    Console.Error.WriteLine($"Failed to select folder: {folderEx.Message}");
                    return;
                }

                // Build a search query (e.g., messages with "Report" in the subject)
                ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                queryBuilder.Subject.Contains("Report");
                MailQuery query = queryBuilder.GetQuery();

                // Retrieve messages that satisfy the query
                ImapMessageInfoCollection matchingMessages = client.ListMessages(query);

                // Output basic information about each matching message
                foreach (ImapMessageInfo messageInfo in matchingMessages)
                {
                    Console.WriteLine($"UID: {messageInfo.UniqueId}, Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
