using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace CreateFolderSample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define service URL and credentials (replace with actual values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client safely
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Get the URI of the parent folder (e.g., Inbox)
                    string parentFolderUri = client.MailboxInfo.InboxUri;

                    // Name of the new folder to create
                    string newFolderName = "MyNewFolder";

                    // Create the folder
                    ExchangeFolderInfo newFolderInfo = client.CreateFolder(parentFolderUri, newFolderName);

                    // Output the result
                    Console.WriteLine("Folder created successfully.");
                    Console.WriteLine("Folder Name: " + newFolderInfo.DisplayName);
                    Console.WriteLine("Folder URI: " + newFolderInfo.Uri);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}