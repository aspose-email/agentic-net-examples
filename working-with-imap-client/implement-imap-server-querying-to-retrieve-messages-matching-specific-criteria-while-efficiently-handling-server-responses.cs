using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // IMAP server connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the folder to search
                    client.SelectFolder("INBOX");

                    // Build the search query (e.g., subject contains "Report" and from a specific address)
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Report");
                    builder.From.Contains("reports@example.com");
                    MailQuery query = builder.GetQuery();

                    // Retrieve up to 100 matching messages asynchronously (synchronously wait for result)
                    ImapMessageInfoCollection messageInfos = client
                        .ListMessagesAsync(query, 100, CancellationToken.None)
                        .GetAwaiter()
                        .GetResult();

                    // Process each message
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message using its unique identifier
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {message.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
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
