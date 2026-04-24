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
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected to avoid real network calls.
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Define the date range for filtering messages.
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate   = new DateTime(2023, 1, 31);

            // Build an IMAP search query for messages whose internal date falls within the range.
            // The query string follows the format required by MailQuery.
            string queryString = $"(InternalDate >= '{startDate:dd-MMM-yyyy}' & InternalDate <= '{endDate:dd-MMM-yyyy}')";
            MailQuery dateRangeQuery = new MailQuery(queryString);

            // Connect to the IMAP server and retrieve matching messages.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder (default folder after connection).
                    client.SelectFolder("INBOX");

                    // List messages that satisfy the date range query.
                    ImapMessageInfoCollection messages = client.ListMessages(dateRangeQuery);

                    Console.WriteLine($"Found {messages.Count} message(s) between {startDate:d} and {endDate:d}.");

                    // Example: output subject of each matched message.
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"- Subject: {info.Subject}");
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
