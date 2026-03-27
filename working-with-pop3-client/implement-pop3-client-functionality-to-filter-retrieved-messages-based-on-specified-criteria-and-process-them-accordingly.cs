using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

namespace Pop3FilterExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection settings
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Initialize POP3 client inside a using block to ensure disposal
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Build a query to filter messages (e.g., subject contains "Invoice")
                        MailQueryBuilder builder = new MailQueryBuilder();
                        builder.Subject.Contains("Invoice");
                        MailQuery query = builder.GetQuery();

                        // Retrieve filtered message infos
                        Pop3MessageInfoCollection messages = client.ListMessages(query);

                        // Process each message
                        foreach (Pop3MessageInfo info in messages)
                        {
                            // Fetch the full message using its unique identifier
                            MailMessage message = client.FetchMessage(info.UniqueId);
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {info.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during POP3 operations: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
