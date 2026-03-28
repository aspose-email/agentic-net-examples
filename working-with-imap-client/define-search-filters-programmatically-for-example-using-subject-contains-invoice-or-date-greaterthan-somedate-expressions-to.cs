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
            // Connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Use ImapClient inside a using block for proper disposal
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Build a search query: Subject contains "Invoice" and SentDate >= 1 Jan 2023
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    DateTime sinceDate = new DateTime(2023, 1, 1);
                    builder.SentDate.Since(sinceDate);
                    MailQuery query = builder.GetQuery();

                    // Retrieve matching messages
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Display basic information about each matched message
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }
}
