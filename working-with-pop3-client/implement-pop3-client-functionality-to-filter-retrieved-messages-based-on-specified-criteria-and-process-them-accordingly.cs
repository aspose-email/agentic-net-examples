using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

namespace Pop3FilterSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server connection settings
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and configure the POP3 client
                using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Build a query to filter messages (e.g., subjects containing "Invoice")
                    MailQueryBuilder queryBuilder = new MailQueryBuilder();
                    queryBuilder.Subject.Contains("Invoice");
                    MailQuery query = queryBuilder.GetQuery();

                    // Retrieve message infos that match the query
                    Pop3MessageInfoCollection messageInfos = pop3Client.ListMessages(query);

                    Console.WriteLine($"Found {messageInfos.Count} message(s) matching the criteria.");

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        // Fetch the full message
                        using (MailMessage message = pop3Client.FetchMessage(info.SequenceNumber))
                        {
                            // Process the message (example: display subject and sender)
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                        }

                        // Optionally delete the processed message from the server
                        pop3Client.DeleteMessage(info.SequenceNumber);
                    }

                    // Commit deletions to the server
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
