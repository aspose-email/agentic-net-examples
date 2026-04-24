using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder server and credentials
            string serverUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution if placeholders are detected
            if (serverUri.Contains("example.com") ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operation.");
                return;
            }

            string folderName = "TestFolder";

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                try
                {
                    // Determine the parent folder URI (Inbox in this case)
                    string parentFolderUri = client.MailboxInfo.InboxUri;

                    // Build the full URI of the folder to delete
                    string folderUri = parentFolderUri.TrimEnd('/') + "/" + folderName;

                    // Delete the empty folder
                    client.DeleteFolder(folderUri);
                    Console.WriteLine($"Folder '{folderName}' deleted.");

                    // Verify that the folder no longer exists
                    bool exists = client.FolderExists(parentFolderUri, folderName);
                    if (!exists)
                    {
                        Console.WriteLine($"Verified: Folder '{folderName}' no longer exists.");
                    }
                    else
                    {
                        Console.WriteLine($"Folder '{folderName}' still exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during folder deletion: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
