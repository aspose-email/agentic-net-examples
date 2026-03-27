using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace EmailFilteringSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize EWS client with placeholder credentials.
                // Replace with actual server URI, username, and password.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Build a query to filter messages whose subject contains "Invoice".
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the query.
                    var messagesInfo = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    // Iterate over the results and fetch full messages to display subjects.
                    foreach (var info in messagesInfo)
                    {
                        // Fetch the full message using its unique URI.
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Output any errors to the error stream.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
