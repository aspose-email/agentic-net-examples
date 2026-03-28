using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Initialize the Exchange client inside a using block for proper disposal
            using (ExchangeClient client = new ExchangeClient(serviceUrl, credential))
            {
                // Define source folder (Inbox) and target folder name
                string sourceFolderUri = client.MailboxInfo.InboxUri;
                string targetFolderName = "TargetFolder";

                // Ensure the target folder exists; create it if it does not
                ExchangeFolderInfo targetFolderInfo;
                if (!client.FolderExists(client.MailboxInfo.RootUri, targetFolderName, out targetFolderInfo))
                {
                    client.CreateFolder(client.MailboxInfo.RootUri, targetFolderName);
                    client.FolderExists(client.MailboxInfo.RootUri, targetFolderName, out targetFolderInfo);
                }

                string destinationFolderUri = targetFolderInfo.Uri;

                // Retrieve all messages from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                // Move each message to the destination folder while preserving metadata
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    client.MoveMessage(msgInfo, destinationFolderUri);
                }

                Console.WriteLine("Messages transferred successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
