using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Source folder (e.g., Inbox). Use the URI from mailbox info.
                string sourceFolderUri = client.MailboxInfo.InboxUri;

                // List all messages in the source folder and its subfolders recursively
                var messages = client.ListMessages(sourceFolderUri, true);

                Console.WriteLine($"Found {messages.Count} messages to archive.");

                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    try
                    {
                        // Archive the message. The overload takes the source folder URI and the message's unique URI.
                        client.ArchiveItem(sourceFolderUri, msgInfo.UniqueUri);
                        Console.WriteLine($"Archived: {msgInfo.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to archive message '{msgInfo.Subject}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
