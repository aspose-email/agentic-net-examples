using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace placeholders with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://example.com/EWS/Exchange.asmx", "username", "password"))
            {
                // Source folder (Inbox) and destination folder (Drafts)
                string sourceFolderUri = client.MailboxInfo.InboxUri;
                string destinationFolderUri = client.MailboxInfo.DraftsUri;

                // Retrieve messages from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                // Move each message to the destination folder
                foreach (ExchangeMessageInfo info in messages)
                {
                    string movedUri = client.MoveItem(info.UniqueUri, destinationFolderUri);
                    Console.WriteLine($"Moved message {info.UniqueUri} to {destinationFolderUri}. New URI: {movedUri}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
