using Aspose.Email.Clients;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;
using System;

namespace ImapDateRangeFilter
{
    class Program
    {
        static void Main()
        {
            // Author: Aspose.Email example – filter IMAP messages by date range.
            // Adjust these credentials and server details as needed.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Define the date range.
            // Inclusive start (>=) and exclusive end (<) boundaries.
            DateTime startDateInclusive = new DateTime(2023, 1, 1);
            DateTime endDateExclusive = new DateTime(2023, 2, 1);

            // Build the query using MailQueryBuilder.
            MailQueryBuilder builder = new MailQueryBuilder();
            builder.SentDate.Since(startDateInclusive);   // SentDate >= startDateInclusive
            builder.SentDate.Before(endDateExclusive);    // SentDate < endDateExclusive
            MailQuery dateRangeQuery = builder.GetQuery();

            try
            {
                // Create and configure the IMAP client.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Select the folder to search (INBOX by default, but explicit selection is clearer).
                    client.SelectFolder("INBOX");

                    // Retrieve messages that match the date range query.
                    ImapMessageInfoCollection messages = client.ListMessages(dateRangeQuery);

                    Console.WriteLine($"Found {messages.Count} message(s) between {startDateInclusive:yyyy-MM-dd} and {endDateExclusive:yyyy-MM-dd} (exclusive).");
                    foreach (ImapMessageInfo info in messages)
                    {
                        // ImapMessageInfo uses the 'Date' property for the message's internal date.
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}, Sent: {info.Date}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during IMAP operation: {ex.Message}");
            }
        }
    }
}
