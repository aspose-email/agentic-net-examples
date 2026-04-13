using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Create the EWS client (replace with actual service URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Source folder URI – using the Inbox in this example
                string sourceFolderUri = client.MailboxInfo.InboxUri;

                // Retrieve all messages from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                // Archive each message individually
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // ArchiveItem overload expects the source folder URI and the message's unique identifier
                    client.ArchiveItem(sourceFolderUri, messageInfo.UniqueUri);
                    Console.WriteLine($"Archived message: {messageInfo.UniqueUri}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
