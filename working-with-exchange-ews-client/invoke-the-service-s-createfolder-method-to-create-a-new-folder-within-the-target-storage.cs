using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailCreateFolderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection details
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder credentials are detected
                if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping folder creation.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Determine the parent folder (Inbox in this example)
                    string parentFolderUri = client.MailboxInfo.InboxUri;

                    // Name of the new folder to create
                    string newFolderName = "MyNewFolder";

                    // Invoke CreateFolder to create the folder under the parent
                    ExchangeFolderInfo createdFolder = client.CreateFolder(parentFolderUri, newFolderName);

                    // Confirmation message
                    Console.WriteLine("Folder created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
