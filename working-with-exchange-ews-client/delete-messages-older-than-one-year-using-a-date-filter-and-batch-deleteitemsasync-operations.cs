using Aspose.Email.Tools.Search;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials guard
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create EWS client with safety guard
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client as IDisposable)
            {
                // Determine inbox folder URI
                string inboxFolder = client.MailboxInfo?.InboxUri ?? "inbox";

                // Build a query for messages older than one year
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                DateTime oneYearAgo = DateTime.Now.AddYears(-1);
                queryBuilder.InternalDate.Before(oneYearAgo);
                MailQuery query = queryBuilder.GetQuery();

                // List messages in the inbox that match the query
                ExchangeMessageInfoCollection messages = null;
                try
                {
                    messages = client.ListMessages(inboxFolder, query);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages older than one year were found.");
                    return;
                }

                // Delete each message using the sync DeleteItem method
                foreach (ExchangeMessageInfo info in messages)
                {
                    try
                    {
                        client.DeleteItem(info.UniqueUri, new DeletionOptions(DeletionType.MoveToDeletedItems));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete message {info.UniqueUri}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Deleted {messages.Count} messages older than one year.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
