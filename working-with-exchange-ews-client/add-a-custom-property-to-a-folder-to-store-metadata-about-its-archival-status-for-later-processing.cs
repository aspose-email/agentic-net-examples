using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                try
                {
                    // Retrieve the Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // Define a custom folder name that encodes archival status
                    string archiveFolderName = "ArchivedFolder";


                    // Skip external calls when placeholder credentials are used
                    if (serviceUrl.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Check if the folder already exists
                    ExchangeFolderInfo existingFolder;
                    bool folderExists = client.FolderExists(inboxUri, archiveFolderName, out existingFolder);

                    if (!folderExists)
                    {
                        // Create the folder under Inbox
                        ExchangeFolderInfo createdFolder = client.CreateFolder(inboxUri, archiveFolderName);
                        Console.WriteLine($"Created folder: {createdFolder.DisplayName}");
                    }
                    else
                    {
                        Console.WriteLine($"Folder already exists: {existingFolder.DisplayName}");
                    }

                    // At this point, the folder name itself serves as metadata indicating archival status.
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
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
