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
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operation.");
                return;
            }

            // Build a query that matches messages sent today
            MailQueryBuilder builder = new MailQueryBuilder();
            DateTime today = DateTime.Today;
            MailQuery todayQuery = builder.SentDate.On(today);
            MailQuery query = builder.GetQuery(); // Not strictly needed; todayQuery already contains the query

            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    client.ValidateCredentials();

                    // Retrieve messages that match the query
                    Pop3MessageInfoCollection messages = client.ListMessages(todayQuery);

                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"Date: {info.Date}");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
