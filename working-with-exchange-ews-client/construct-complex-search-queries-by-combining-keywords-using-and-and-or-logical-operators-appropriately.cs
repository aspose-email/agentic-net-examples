using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace ComplexMailQuerySample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS connection parameters
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client (implements IDisposable)
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information (folders URIs)
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Construct a complex MailQuery with AND (&) and OR (|) operators
                    // Example: (From contains 'test@test.com' OR Seen = True) AND SentDate >= 12-May-2010
                    MailQuery mailQuery = new MailQuery("(('From' Contains 'test@test.com' | 'Seen' = 'True') & 'SentDate' >= '12-May-2010')");

                    // Search messages in the Inbox that satisfy the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri, mailQuery);

                    Console.WriteLine($"Found {messages.Count} message(s) matching the complex query.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
