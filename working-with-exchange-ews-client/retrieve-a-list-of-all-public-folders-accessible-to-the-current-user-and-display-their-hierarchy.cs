using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace PublicFolderHierarchySample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve root public folders
                    ExchangeFolderInfoCollection publicFolders = client.ListPublicFolders();

                    // Display hierarchy
                    foreach (ExchangeFolderInfo rootFolder in publicFolders)
                    {
                        DisplayFolder(rootFolder, "", client);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        private static void DisplayFolder(ExchangeFolderInfo folder, string indent, IEWSClient client)
        {
            // Print current folder
            Console.WriteLine($"{indent}{folder.DisplayName}");

            // Retrieve subfolders of the current folder
            ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folder);

            // Recursively display each subfolder
            foreach (ExchangeFolderInfo subFolder in subFolders)
            {
                DisplayFolder(subFolder, indent + "    ", client);
            }
        }
    }
}
