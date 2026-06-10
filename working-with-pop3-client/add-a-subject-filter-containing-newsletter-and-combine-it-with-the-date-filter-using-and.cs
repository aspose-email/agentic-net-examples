using Aspose.Email;
using System;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static void Main()
    {
        try
        {
            string host = "pop3.example.com";
            string username = "user";
            string password = "pass";

            // Skip real network calls when placeholder credentials are used
            if (host.Contains("example") || username == "user" || password == "pass")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operations.");
                return;
            }

            // Create and use POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();

                    // Build query: subject contains "newsletter" AND sent date within last 7 days
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("newsletter");
                    builder.SentDate.Since(DateTime.UtcNow.AddDays(-7));
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
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
