using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Create an IMAP query builder instance
            ImapQueryBuilder imapBuilder = new ImapQueryBuilder();

            // Add search criteria
            imapBuilder.From.Contains("alice@example.com");
            imapBuilder.Subject.Contains("Quarterly Report");
            imapBuilder.Body.Contains("confidential");

            // Build the first query
            MailQuery firstQuery = imapBuilder.GetQuery();

            // Create a second query using the base MailQueryBuilder
            MailQueryBuilder secondBuilder = new MailQueryBuilder();
            secondBuilder.Subject.Contains("Invoice");
            MailQuery secondQuery = secondBuilder.GetQuery();

            // Combine the two queries with OR
            MailQuery combinedQuery = imapBuilder.Or(firstQuery, secondQuery);

            // The combinedQuery can now be used with ImapClient.ListMessages(combinedQuery)
            Console.WriteLine("IMAP query constructed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}