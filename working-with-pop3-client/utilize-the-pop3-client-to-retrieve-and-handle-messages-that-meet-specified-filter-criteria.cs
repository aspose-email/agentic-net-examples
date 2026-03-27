using Aspose.Email.Clients;
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
            // POP3 server connection parameters (replace with real values)
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Create and dispose the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Build a query to filter messages (e.g., subject contains "Report")
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Report");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages that match the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    foreach (Pop3MessageInfo info in messages)
                    {
                        // Fetch the full message using its unique identifier
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                        }
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
        }
    }
}
