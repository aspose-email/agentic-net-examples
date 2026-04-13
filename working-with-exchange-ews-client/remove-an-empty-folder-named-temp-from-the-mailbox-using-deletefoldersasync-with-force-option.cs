using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using System.Collections.Specialized;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip network call if they are not replaced.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                username == "username" ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping mailbox operation.");
                return;
            }

            // Create EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Attempt to get information about the 'Temp' folder.
                ExchangeFolderInfo tempFolderInfo;
                try
                {
                    tempFolderInfo = client.GetFolderInfo("Temp");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to locate folder 'Temp': {ex.Message}");
                    return;
                }

                // Prepare collection of folder URIs to delete.
                StringCollection folderUris = new StringCollection { tempFolderInfo.Uri };

                // Delete the folder permanently (force option).
                try
                {
                    client.DeleteFolders(folderUris, true);
                    Console.WriteLine("Folder 'Temp' deleted permanently.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
