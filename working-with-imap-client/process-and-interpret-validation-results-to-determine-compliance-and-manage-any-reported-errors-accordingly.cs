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
            // IMAP server connection parameters (replace with real values or keep placeholders)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the IMAP client inside a using block to ensure disposal
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the folder to search (e.g., INBOX)
                    client.SelectFolder("INBOX");

                    // Build a search query: find messages whose subject contains "Report"
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Report");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages that match the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Iterate through the results
                    foreach (ImapMessageInfo info in messages)
                    {
                        // Fetch the full message using its unique identifier
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                            Console.WriteLine($"From: {info.From}");
                            Console.WriteLine($"Date: {info.Date}");
                            // Additional processing can be done here
                        }
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
