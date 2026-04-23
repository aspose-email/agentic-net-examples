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
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Start from the Inbox folder as the root for demonstration.
                string rootFolderUri = client.MailboxInfo.InboxUri;

                Console.WriteLine("Folder hierarchy:");
                PrintFolderHierarchy(client, rootFolderUri, 0);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively prints folder names with indentation to represent hierarchy.
    private static void PrintFolderHierarchy(ExchangeClient client, string folderUri, int level)
    {
        try
        {
            // Retrieve subfolders of the current folder.
            ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri);

            foreach (ExchangeFolderInfo folderInfo in subFolders)
            {
                // Indent according to depth level.
                string indent = new string(' ', level * 2);
                Console.WriteLine($"{indent}- {folderInfo.DisplayName}");

                // Recurse into the subfolder.
                PrintFolderHierarchy(client, folderInfo.Uri, level + 1);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to list subfolders for '{folderUri}': {ex.Message}");
        }
    }
}
