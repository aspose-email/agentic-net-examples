using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Build a MailQuery to filter messages (e.g., subject contains "Report")
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.Subject.Contains("Report", ignoreCase: true);
                MailQuery query = queryBuilder.GetQuery();

                // Retrieve messages matching the query
                Pop3MessageInfoCollection messages = client.ListMessages(query);
                int totalMessages = messages.Count;

                // Log the total number of messages retrieved
                Console.WriteLine($"Total messages matching query: {totalMessages}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
