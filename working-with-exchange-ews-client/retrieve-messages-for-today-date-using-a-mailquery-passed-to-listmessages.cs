using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Replace with your actual Exchange server details
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and connect the EWS client
                IEWSClient client = null;
                try
                {
                    client = EWSClient.GetEWSClient(mailboxUri, username, password);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                    return;
                }

                // Build a MailQuery for messages received today
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                // Messages with InternalDate >= today 00:00 and < tomorrow 00:00
                queryBuilder.InternalDate.Since(DateTime.Today);
                queryBuilder.InternalDate.Before(DateTime.Today.AddDays(1));
                MailQuery todayQuery = queryBuilder.GetQuery();

                // Retrieve messages from the Inbox folder that match the query
                ExchangeMessageInfoCollection messages = null;
                try
                {
                    // Use the Inbox folder URI from the mailbox info
                    string inboxFolderUri = client.MailboxInfo.InboxUri;
                    messages = client.ListMessages(inboxFolderUri, todayQuery);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                    return;
                }

                // Display basic information about each message
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                    Console.WriteLine($"Received: {messageInfo.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }

                // Dispose the client
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
