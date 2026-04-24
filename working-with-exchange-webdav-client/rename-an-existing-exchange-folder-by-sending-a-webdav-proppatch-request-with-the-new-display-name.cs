using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real server details.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against running with placeholder credentials.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // The folder to rename (full URI or distinguished name).
                string targetFolderUri = "/exchange/user@example.com/Inbox/OldFolder";

                // New display name for the folder.
                string newDisplayName = "RenamedFolder";

                // Retrieve information about the target folder.
                ExchangeFolderInfo folderInfo;
                try
                {
                    folderInfo = client.GetFolderInfo(targetFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get folder info: {ex.Message}");
                    return;
                }

                // Derive the parent folder URI from the current folder URI.
                // Example: "/exchange/user@example.com/Inbox/OldFolder" -> "/exchange/user@example.com/Inbox"
                string parentFolderUri = Path.GetDirectoryName(folderInfo.Uri.Replace('/', Path.DirectorySeparatorChar))
                                         .Replace(Path.DirectorySeparatorChar, '/');

                // Create a new folder with the desired display name under the same parent.
                try
                {
                    client.CreateFolder(parentFolderUri, newDisplayName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create new folder: {ex.Message}");
                    return;
                }

                // Build the URI of the newly created folder.
                string newFolderUri = $"{parentFolderUri}/{newDisplayName}";

                // Move all items from the old folder to the new folder.
                try
                {
                    // Retrieve all message URIs from the old folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(targetFolderUri);
                    foreach (var msgUri in messages)
                    {
                        client.MoveMessage(msgUri, newFolderUri);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to move messages: {ex.Message}");
                    // Continue to attempt deletion of the old folder.
                }

                // Delete the old folder.
                try
                {
                    client.DeleteFolder(targetFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete old folder: {ex.Message}");
                }

                Console.WriteLine($"Folder renamed to '{newDisplayName}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
