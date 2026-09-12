using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AqsQuerySample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Author: Sample demonstrating AQS query construction and usage with EWS
                // Initialize EWS client (replace with real credentials)
                string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Build an Advanced Query Syntax (AQS) query
                    // Example: messages from test@example.com that are unread and received after 1 Jan 2023
                    string aqsExpression = "(From:'test@example.com' AND IsRead:false AND SentDate>='2023-01-01')";

                    // Skip external calls when placeholder credentials are used
                    if (username.Contains("example.com") || password == "password" || aqsExpression.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    ExchangeAdvancedSyntaxMailQuery query = new ExchangeAdvancedSyntaxMailQuery(aqsExpression);

                    // Get the Inbox folder URI
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // Search messages in the Inbox using the AQS query
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query);

                    Console.WriteLine($"Found {messages.Count} message(s) matching the AQS query.");

                    // Optionally fetch and display subject of each message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Fetch the full message as a MailMessage
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
