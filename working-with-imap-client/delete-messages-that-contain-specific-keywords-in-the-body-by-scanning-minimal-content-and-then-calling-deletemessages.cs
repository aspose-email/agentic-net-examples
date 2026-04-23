using Aspose.Email.Tools.Search;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Build a query that matches messages containing any of the keywords in the body
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    MailQuery keywordQuery1 = queryBuilder.Body.Contains("keyword1");
                    MailQuery keywordQuery2 = queryBuilder.Body.Contains("keyword2");
                    MailQuery combinedQuery = queryBuilder.Or(keywordQuery1, keywordQuery2);

                    // Retrieve messages that match the query
                    ImapMessageInfoCollection matchedMessages = client.ListMessages(combinedQuery);

                    // Delete the matched messages
                    if (matchedMessages != null && matchedMessages.Count > 0)
                    {
                        client.DeleteMessages(matchedMessages);
                        Console.WriteLine($"{matchedMessages.Count} message(s) deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the specified keywords.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
