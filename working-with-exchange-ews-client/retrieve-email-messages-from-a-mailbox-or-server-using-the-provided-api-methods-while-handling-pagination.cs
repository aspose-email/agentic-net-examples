using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client (automatically handles connection)
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Obtain mailbox information to get the Inbox URI
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                string inboxUri = mailboxInfo.InboxUri;

                // Build a query that matches all messages
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                MailQuery query = queryBuilder.GetQuery();

                const int itemsPerPage = 50;

                while (true)
                {
                    // Retrieve a page of messages from the Inbox
                    ExchangeMessagePageInfo pageInfo = client.ListMessagesByPage(inboxUri, query, itemsPerPage);
                    if (pageInfo == null || pageInfo.Items == null || pageInfo.Items.Count == 0)
                    {
                        break; // No more messages
                    }

                    // Process each message in the current page
                    foreach (ExchangeMessageInfo msgInfo in pageInfo.Items)
                    {
                        // Fetch the full message using its unique URI
                        using (MailMessage message = client.FetchMessage(msgInfo.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }

                    // If the page returned fewer items than requested, we have reached the end
                    if (pageInfo.Items.Count < itemsPerPage)
                    {
                        break;
                    }

                    // Note: ListMessagesByPage does not accept a page index; for a real pagination loop,
                    // adjust the query (e.g., with Skip/Take) or use other paging mechanisms.
                    // This example stops after the first page for simplicity.
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
