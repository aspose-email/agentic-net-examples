using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsDeleteFolderExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Replace with your actual Exchange Web Services URL and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client (returns an IEWSClient implementation)
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Obtain mailbox information to retrieve a folder URI.
                    // In a real scenario, replace mailboxInfo.InboxUri with the URI of the folder you intend to delete.
                    var mailboxInfo = client.GetMailboxInfo();
                    string folderUri = mailboxInfo.InboxUri;

                    // Delete the specified folder. This moves the folder to Deleted Items.
                    client.DeleteFolder(folderUri);

                    Console.WriteLine($"Folder '{folderUri}' has been deleted (moved to Deleted Items).");
                }
            }
            catch (Exception ex)
            {
                // Gracefully report any errors.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
