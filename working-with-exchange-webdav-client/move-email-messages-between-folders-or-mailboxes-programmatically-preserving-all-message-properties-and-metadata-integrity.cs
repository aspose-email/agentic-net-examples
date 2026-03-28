using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string userName = "user@example.com";
            string password = "password";

            // Initialize the Exchange client inside a using block to ensure disposal
            using (ExchangeClient client = new ExchangeClient(serviceUrl, userName, password))
            {
                // Define source and destination folder URIs
                string sourceFolderUri = client.MailboxInfo.InboxUri;
                string destinationFolderUri = client.MailboxInfo.SentItemsUri; // example destination

                // Retrieve all messages from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                // Move each message to the destination folder, preserving all properties
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    client.MoveMessage(msgInfo, destinationFolderUri);
                    Console.WriteLine($"Moved message URI: {msgInfo.UniqueUri}");
                }
            }
        }
        catch (Exception ex)
        {
            // Output any errors without crashing the application
            Console.Error.WriteLine(ex.Message);
        }
    }
}
