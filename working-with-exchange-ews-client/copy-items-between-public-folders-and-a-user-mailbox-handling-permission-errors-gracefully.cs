using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using AspNetCore = Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (preserve variable name "client")
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Define source public folder name and destination folder URI (e.g., Inbox)
                string sourcePublicFolderName = "PublicFolder";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                string destinationFolderUri = client.MailboxInfo.InboxUri;

                // Locate the source public folder
                ExchangeFolderInfo sourceFolder = null;
                foreach (ExchangeFolderInfo folder in client.ListPublicFolders())
                {
                    if (string.Equals(folder.DisplayName, sourcePublicFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        sourceFolder = folder;
                        break;
                    }
                }

                if (sourceFolder == null)
                {
                    Console.Error.WriteLine($"Public folder \"{sourcePublicFolderName}\" not found.");
                    return;
                }

                // Retrieve messages from the public folder
                ExchangeMessageInfoCollection messages = client.ListMessagesFromPublicFolder(sourceFolder);

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    try
                    {
                        // Copy each message to the destination folder
                        string copiedItemUri = client.CopyItem(messageInfo.UniqueUri, destinationFolderUri);
                        Console.WriteLine($"Copied message. New URI: {copiedItemUri}");
                    }
                    catch (Exception ex)
                    {
                        // Handle permission errors or other copy failures gracefully
                        Console.Error.WriteLine($"Failed to copy message {messageInfo.UniqueUri}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
