using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials detection – skip execution in CI environments
                string host = "imap.gmail.com";
                int port = 993;
                string username = "your.email@gmail.com";
                string password = "yourpassword";

                if (host.Contains("example") || username.Contains("example") || password.Contains("example"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Connect to Gmail via IMAP
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Validate the connection credentials
                    client.ValidateCredentials();

                    // Build a query for messages received in the last 7 days from a specific domain
                    DateTime since = DateTime.UtcNow.AddDays(-7);
                    string queryString = $"('SentDate' >= '{since:dd-MMM-yyyy}' AND 'From' Contains '@example.com')";
                    MailQuery query = new MailQuery(queryString);

                    // Retrieve matching messages
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Output basic information about each message
                    foreach (ImapMessageInfo info in messages)
                    {
                        string fromAddresses = string.Join(", ", info.From);
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {fromAddresses}");
                        Console.WriteLine($"Date: {info.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
