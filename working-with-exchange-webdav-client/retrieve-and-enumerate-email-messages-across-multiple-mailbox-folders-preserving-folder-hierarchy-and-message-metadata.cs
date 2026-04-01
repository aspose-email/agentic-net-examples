using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – real values should be provided for actual execution
            string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (serverUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create and use the Exchange WebDav client
            using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
            {
                try
                {
                    // Start enumeration from the root folder (Inbox) – you can change this to any known folder URI
                    string rootFolderUri = client.MailboxInfo.InboxUri;
                    EnumerateFolder(client, rootFolderUri, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during mailbox enumeration: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively enumerates folders and their messages, preserving hierarchy via indentation
    private static void EnumerateFolder(ExchangeClient client, string folderUri, string indent)
    {
        // Retrieve folder information
        ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderUri);

        // Display folder name (fallback to URI if DisplayName is unavailable)
        string folderName = !string.IsNullOrEmpty(folderInfo.DisplayName) ? folderInfo.DisplayName : folderInfo.Uri;
        Console.WriteLine($"{indent}Folder: {folderName}");

        // List messages in the current folder
        ExchangeMessageInfoCollection messages = client.ListMessages(folderInfo.Uri);
        foreach (ExchangeMessageInfo msgInfo in messages)
        {
            // Use InternalDate as per validation rule
            Console.WriteLine($"{indent}  Subject: {msgInfo.Subject}");
            Console.WriteLine($"{indent}  From: {msgInfo.From}");
            Console.WriteLine($"{indent}  Date (Internal): {msgInfo.InternalDate}");
        }

        // Recursively process subfolders
        ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderInfo);
        foreach (ExchangeFolderInfo subFolder in subFolders)
        {
            EnumerateFolder(client, subFolder.Uri, indent + "  ");
        }
    }
}
