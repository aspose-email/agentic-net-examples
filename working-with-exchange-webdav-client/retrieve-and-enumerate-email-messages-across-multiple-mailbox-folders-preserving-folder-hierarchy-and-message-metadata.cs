using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Server connection parameters (replace with real values)
            string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the Exchange WebDav client
            using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
            {
                try
                {
                    // Start enumeration from the Inbox folder
                    string inboxUri = client.MailboxInfo.InboxUri;
                    EnumerateFolder(client, inboxUri, 0);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during folder enumeration: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively enumerates a folder, its messages and subfolders
    private static void EnumerateFolder(ExchangeClient client, string folderUri, int depth)
    {
        // Get folder information
        ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderUri);
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}Folder: {folderInfo.DisplayName}");

        // List messages in the current folder (non‑recursive)
        ExchangeMessageInfoCollection messages = client.ListMessages(folderUri, false);
        foreach (ExchangeMessageInfo messageInfo in messages)
        {
            // Fetch the full message to read metadata
            MailMessage message = client.FetchMessage(messageInfo.UniqueUri);
            Console.WriteLine($"{indent}  Message Subject: {message.Subject}");
            Console.WriteLine($"{indent}  From: {message.From}");
            Console.WriteLine($"{indent}  Received: {message.Date}");
        }

        // List subfolders and recurse
        ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri);
        foreach (ExchangeFolderInfo subFolderInfo in subFolders)
        {
            EnumerateFolder(client, subFolderInfo.Uri, depth + 1);
        }
    }
}
