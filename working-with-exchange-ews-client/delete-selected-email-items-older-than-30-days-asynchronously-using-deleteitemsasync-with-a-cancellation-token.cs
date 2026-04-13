using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");

            // Create a cancellation token (not used by sync API but shown for completeness)
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Get the Inbox folder URI
                        string inboxUri = client.MailboxInfo.InboxUri;

                        // Retrieve all messages in the Inbox
                        IEnumerable<ExchangeMessageInfo> messages = client.ListMessages(inboxUri);

                        // Select messages older than 30 days
                        List<string> oldMessageUris = new List<string>();
                        DateTime cutoffDate = DateTime.Now.AddDays(-30);
                        foreach (ExchangeMessageInfo msgInfo in messages)
                        {
                            if (msgInfo.InternalDate < cutoffDate)
                            {
                                oldMessageUris.Add(msgInfo.UniqueUri);
                            }
                        }

                        if (oldMessageUris.Count > 0)
                        {
                            // Delete the selected messages (move to Deleted Items)
                            DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                            client.DeleteItems(oldMessageUris, deleteOptions);
                            Console.WriteLine($"{oldMessageUris.Count} message(s) older than 30 days were deleted.");
                        }
                        else
                        {
                            Console.WriteLine("No messages older than 30 days were found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
