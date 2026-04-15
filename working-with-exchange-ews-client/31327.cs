using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange server URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string customFolderName = "MyCustomFolder";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Get Inbox folder information
                ExchangeFolderInfo inboxInfo = client.GetFolderInfo("inbox");

                // Ensure the custom folder exists under the Inbox; create if missing
                ExchangeFolderInfo customFolderInfo;
                bool folderExists = client.FolderExists(inboxInfo.Uri, customFolderName, out customFolderInfo);
                if (!folderExists)
                {
                    client.CreateFolder(inboxInfo.Uri, customFolderName);
                    // Retrieve the newly created folder info
                    customFolderInfo = client.GetFolderInfo(customFolderName);
                }

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxInfo.Uri);
                if (messages.Count > 0)
                {
                    // Take the first message as an example
                    ExchangeMessageInfo messageInfo = messages[0];

                    // Move the message to the custom folder
                    string movedMessageUri = client.MoveItem(messageInfo.UniqueUri, customFolderInfo.Uri);
                    Console.WriteLine($"Message moved successfully. New URI: {movedMessageUri}");
                }
                else
                {
                    Console.WriteLine("Inbox is empty. No message to move.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
