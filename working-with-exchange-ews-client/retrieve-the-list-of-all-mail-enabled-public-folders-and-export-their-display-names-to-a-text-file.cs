using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace PublicFolderExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange server connection settings
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Output file path
                string outputPath = "PublicFolders.txt";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine("Failed to prepare output directory: " + dirEx.Message);
                    return;
                }

                // Create and use the EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                    {
                        // Retrieve public folders
                        ExchangeFolderInfoCollection publicFolders = client.ListPublicFolders();

                        // Write display names to the text file
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(outputPath, false))
                            {
                                foreach (ExchangeFolderInfo folderInfo in publicFolders)
                                {
                                    writer.WriteLine(folderInfo.DisplayName);
                                }
                            }
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine("Error writing to file: " + writeEx.Message);
                            return;
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine("Error accessing Exchange server: " + clientEx.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
