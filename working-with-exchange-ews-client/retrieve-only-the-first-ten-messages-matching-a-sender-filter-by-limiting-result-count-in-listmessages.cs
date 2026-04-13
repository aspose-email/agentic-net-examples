using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // EWS connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username   = "user@example.com";
            string password   = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a query to filter messages from a specific sender
                ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                queryBuilder.From.Contains("sender@example.com");
                MailQuery query = queryBuilder.GetQuery();

                // Folder URI for the Inbox
                string inboxFolderUri = client.MailboxInfo.InboxUri;

                // Retrieve the first 10 messages that match the sender filter
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxFolderUri, 10, query);

                // Display basic information about each retrieved message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From   : {info.From}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
