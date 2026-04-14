using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Keyword to search in the subject line
            string keyword = "Important";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Get the Inbox folder URI
                string inboxFolderUri = client.MailboxInfo.InboxUri;

                // Retrieve all messages from the Inbox
                IEnumerable<ExchangeMessageInfo> messages = client.ListMessages(inboxFolderUri);

                // Collect URIs of messages whose subject contains the keyword
                List<string> itemsToDelete = new List<string>();
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    if (!string.IsNullOrEmpty(msgInfo.Subject) &&
                        msgInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        itemsToDelete.Add(msgInfo.UniqueUri);
                    }
                }

                // Delete the matched messages
                if (itemsToDelete.Count > 0)
                {
                    DeletionOptions options = new DeletionOptions(DeletionType.MoveToDeletedItems);
                    client.DeleteItems(itemsToDelete, options);
                    Console.WriteLine($"{itemsToDelete.Count} message(s) deleted.");
                }
                else
                {
                    Console.WriteLine("No messages matched the keyword.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
