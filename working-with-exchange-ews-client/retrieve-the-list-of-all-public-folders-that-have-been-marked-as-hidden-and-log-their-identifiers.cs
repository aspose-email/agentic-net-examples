using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client (replace with actual server URI and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve all public folders
                ExchangeFolderInfoCollection publicFolders = client.ListPublicFolders();

                // Iterate through the collection and log hidden folders
                foreach (ExchangeFolderInfo folderInfo in publicFolders)
                {
                    if (folderInfo.Hidden)
                    {
                        Console.WriteLine($"Hidden Public Folder URI: {folderInfo.Uri}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
