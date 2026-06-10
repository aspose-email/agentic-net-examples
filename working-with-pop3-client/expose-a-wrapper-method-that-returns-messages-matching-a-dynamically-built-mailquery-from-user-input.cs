using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // User input for query criteria
            Console.Write("Enter sender email to filter (or leave empty): ");
            string fromFilter = Console.ReadLine();

            Console.Write("Enter subject keyword to filter (or leave empty): ");
            string subjectFilter = Console.ReadLine();

            // Build the MailQuery based on user input
            MailQuery query = BuildMailQuery(fromFilter, subjectFilter);

            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholders are detected
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server connection.");
                return;
            }

            // Retrieve matching messages
            List<Pop3MessageInfo> messages = await GetMessagesAsync(host, port, username, password, query);

            Console.WriteLine($"Found {messages.Count} message(s) matching the query.");
            foreach (Pop3MessageInfo info in messages)
            {
                Console.WriteLine($"- UID: {info.UniqueId}, Subject: {info.Subject}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Builds a MailQuery using MailQueryBuilder based on provided filters
    private static MailQuery BuildMailQuery(string fromFilter, string subjectFilter)
    {
        MailQueryBuilder builder = new MailQueryBuilder();

        if (!string.IsNullOrWhiteSpace(fromFilter))
        {
            // Case‑insensitive contains on the From field
            builder.From.Contains(fromFilter, true);
        }

        if (!string.IsNullOrWhiteSpace(subjectFilter))
        {
            // Case‑insensitive contains on the Subject field
            builder.Subject.Contains(subjectFilter, true);
        }

        return builder.GetQuery();
    }

    // Wrapper that connects to POP3 server and returns messages matching the query
    private static Task<List<Pop3MessageInfo>> GetMessagesAsync(
        string host,
        int port,
        string username,
        string password,
        MailQuery query)
    {
        return Task.Run(() =>
        {
            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Validate credentials before proceeding
                    client.ValidateCredentials();

                    // Retrieve messages that satisfy the query
                    Pop3MessageInfoCollection infoCollection = client.ListMessages(query);
                    return new List<Pop3MessageInfo>(infoCollection);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve messages: {ex.Message}");
                return new List<Pop3MessageInfo>();
            }
        });
    }
}
