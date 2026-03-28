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
            // IMAP server connection parameters (replace with real values if available)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Define the date range (inclusive)
            DateTime startDate = new DateTime(2022, 1, 1);
            DateTime endDate   = new DateTime(2022, 1, 31);

            // Build the query string using the required format: d-MMM-yyyy (e.g., 01-Jan-2022)
            string queryString = string.Format(
                "('InternalDate' >= '{0:dd-MMM-yyyy}' & 'InternalDate' <= '{1:dd-MMM-yyyy}')",
                startDate, endDate);

            // Create a MailQuery with the constructed query string
            MailQuery dateRangeQuery = new MailQuery(queryString);

            // Connect to the IMAP server and fetch messages that match the date range
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Retrieve the list of messages that satisfy the query
                ImapMessageInfoCollection messages = client.ListMessages(dateRangeQuery);

                Console.WriteLine($"Found {messages.Count} message(s) between {startDate:d} and {endDate:d}:");

                foreach (ImapMessageInfo info in messages)
                {
                    // Fetch the full message to access its properties (e.g., Subject)
                    using (MailMessage message = client.FetchMessage(info.UniqueId))
                    {
                        Console.WriteLine($"- Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully report any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
