using Aspose.Email.Clients;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to run against an actual server.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing live network calls with placeholder data.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping server query.");
                return;
            }

            // Create and connect the IMAP client inside a using block to ensure disposal.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Build a search query: messages whose subject contains "Report".
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Report");
                    MailQuery query = builder.GetQuery();

                    // Retrieve up to 100 matching messages asynchronously, then wait for the result.
                    ImapMessageInfoCollection messageInfos = client.ListMessagesAsync(query, 100, CancellationToken.None).Result;

                    // Iterate through the results and fetch each message's subject.
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message using its unique identifier.
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {message.Subject}");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
