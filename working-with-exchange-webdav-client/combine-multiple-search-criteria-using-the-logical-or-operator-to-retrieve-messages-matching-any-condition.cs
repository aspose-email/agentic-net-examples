using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution in CI.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid external network calls.
                if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                    return;
                }

                // Create and connect the Exchange WebDAV client.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Build first search criterion: Subject contains "Report".
                        ExchangeQueryBuilder subjectBuilder = new ExchangeQueryBuilder();
                        subjectBuilder.Subject.Contains("Report");
                        MailQuery subjectQuery = subjectBuilder.GetQuery();

                        // Build second search criterion: From contains "sales@example.com".
                        ExchangeQueryBuilder fromBuilder = new ExchangeQueryBuilder();
                        fromBuilder.From.Contains("sales@example.com");
                        MailQuery fromQuery = fromBuilder.GetQuery();

                        // Combine the two criteria with logical OR.
                        ExchangeQueryBuilder combineBuilder = new ExchangeQueryBuilder();
                        MailQuery combinedQuery = combineBuilder.Or(subjectQuery, fromQuery);

                        // Retrieve messages from the Inbox that match either condition.
                        string inboxUri = client.MailboxInfo.InboxUri;
                        ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, combinedQuery, true);

                        // Output basic information about each matching message.
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                            Console.WriteLine($"From: {messageInfo.From}");
                            Console.WriteLine($"Date: {messageInfo.InternalDate}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
                return;
            }
        }
    }
}
