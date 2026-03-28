using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailImapSearch
{
    class Program
    {
        static void Main()
        {
            try
            {
                // IMAP server connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                SecurityOptions security = SecurityOptions.Auto;

                // Create and connect the IMAP client inside a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, port, username, password, security))
                {
                    // Build a search query for messages whose subject contains the specified text
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Your Subject Text");
                    MailQuery query = builder.GetQuery();

                    // Retrieve the list of messages that match the query
                    ImapMessageInfoCollection infos = client.ListMessages(query);

                    Console.WriteLine($"Found {infos.Count} message(s) matching the subject filter.");

                    // Iterate through the matched messages and fetch full details
                    foreach (ImapMessageInfo info in infos)
                    {
                        // Fetch the full mail message using the unique identifier
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (ImapException imapEx)
            {
                Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
