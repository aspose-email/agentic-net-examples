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
            // Placeholder IMAP server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Define the date range (inclusive)
                    DateTime startDate = new DateTime(2023, 1, 1);
                    DateTime endDate = new DateTime(2023, 12, 31);

                    // Build a MailQuery for the date range
                    string queryString = $"('SentDate' >= '{startDate:dd-MMM-yyyy}' AND 'SentDate' <= '{endDate:dd-MMM-yyyy}')";
                    MailQuery query = new MailQuery(queryString);

                    // Retrieve messages that match the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Output basic information about each message
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"Sent Date: {info.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
