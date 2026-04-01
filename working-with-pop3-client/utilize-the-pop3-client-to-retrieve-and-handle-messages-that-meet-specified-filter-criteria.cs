using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder POP3 connection settings
            string host = "pop.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network call.");
                return;
            }

            // Build a query to filter messages (e.g., subject contains "Invoice")
            MailQueryBuilder queryBuilder = new MailQueryBuilder();
            queryBuilder.Subject.Contains("Invoice");
            MailQuery query = queryBuilder.GetQuery();

            // Use POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate credentials
                    client.ValidateCredentials();

                    // List messages that satisfy the query
                    Pop3MessageInfoCollection infos = client.ListMessages(query);
                    foreach (Pop3MessageInfo info in infos)
                    {
                        // Fetch the full message
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            // Process the message (display subject and size)
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"Size: {info.Size} bytes");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
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
