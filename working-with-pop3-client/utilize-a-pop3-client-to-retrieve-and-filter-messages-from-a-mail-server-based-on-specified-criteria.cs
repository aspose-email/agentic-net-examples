using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder POP3 server credentials
            string host = "pop.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip real network call when using placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Skipping POP3 operations due to placeholder credentials.");
                return;
            }

            // Create and configure the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.None))
            {
                try
                {
                    // Build a query to filter messages (e.g., subject contains "Invoice")
                    MailQueryBuilder queryBuilder = new MailQueryBuilder();
                    queryBuilder.Subject.Contains("Invoice");
                    MailQuery query = queryBuilder.GetQuery();

                    // Retrieve filtered message infos
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    // Process each message info
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Date: {info.Date}");
                        Console.WriteLine($"Size: {info.Size} bytes");
                        Console.WriteLine(new string('-', 40));
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
