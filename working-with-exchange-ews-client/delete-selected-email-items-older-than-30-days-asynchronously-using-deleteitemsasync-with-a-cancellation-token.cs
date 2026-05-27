using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Folder to process (Inbox)
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve all item URIs from the Inbox
                string[] itemUris = client.ListItems(inboxUri);

                // Determine the cutoff date (30 days ago)
                DateTime cutoffDate = DateTime.UtcNow.AddDays(-30);
                List<string> oldItemUris = new List<string>();

                // Filter items older than the cutoff date
                foreach (string uri in itemUris)
                {
                    MailMessage message = client.FetchMessage(uri);
                    if (message.Date < cutoffDate)
                    {
                        oldItemUris.Add(uri);
                    }
                }

                if (oldItemUris.Count > 0)
                {
                    // Delete the selected items (move to Deleted Items)
                    DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                    client.DeleteItems(oldItemUris, deleteOptions);
                    Console.WriteLine($"{oldItemUris.Count} items older than 30 days were deleted.");
                }
                else
                {
                    Console.WriteLine("No items older than 30 days were found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
