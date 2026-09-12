using Aspose.Email.Storage.Pst;
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
            // Placeholder credentials – replace with real values for actual execution
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard: skip network operations when placeholders are detected
            if (username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client (connection is established inside the using block)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the Inbox folder information – this will be the parent for the new folder
                ExchangeFolderInfo inboxInfo = client.GetFolderInfo("inbox");
                string parentFolderUri = inboxInfo.Uri;

                // Create a custom folder under the Inbox
                string customFolderName = "MyCustomFolder";
                ExchangeFolderInfo customFolder = client.CreateFolder(parentFolderUri, customFolderName);
                Console.WriteLine($"Created folder '{customFolderName}' with URI: {customFolder.Uri}");

                // Verify the folder exists using the LIST (FolderExists) command
                bool folderExists = client.FolderExists(parentFolderUri, customFolderName);
                Console.WriteLine($"Folder existence check: {folderExists}");

                // List messages in the Inbox and move a few of them to the custom folder
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(parentFolderUri);
                int movedCount = 0;
                foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                {
                    if (movedCount >= 2) break; // move only the first two messages as an example

                    // Move the message to the newly created folder
                    client.MoveItem(messageInfo.UniqueUri, customFolder.Uri);
                    movedCount++;
                }
                Console.WriteLine($"Moved {movedCount} message(s) to '{customFolderName}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
