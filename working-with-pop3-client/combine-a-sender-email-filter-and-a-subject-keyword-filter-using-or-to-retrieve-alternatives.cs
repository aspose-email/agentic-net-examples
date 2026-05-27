using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 server detected. Skipping connection.");
                return;
            }

            // Create and connect POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate credentials: {ex.Message}");
                    return;
                }

                // Build query: messages from specific sender OR with specific subject keyword
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                MailQuery fromQuery = queryBuilder.From.Contains("sender@example.com");
                MailQuery subjectQuery = queryBuilder.Subject.Contains("Important");
                MailQuery combinedQuery = queryBuilder.Or(fromQuery, subjectQuery);

                // Retrieve messages matching the combined query
                Pop3MessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages(combinedQuery);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Output basic info for each matching message
                foreach (Pop3MessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Sequence #: {info.SequenceNumber}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
