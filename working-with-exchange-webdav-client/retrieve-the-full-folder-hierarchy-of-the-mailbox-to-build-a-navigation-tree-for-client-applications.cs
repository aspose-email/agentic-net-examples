using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details – replace with real values.
                string exchangeUrl = "https://exchange.example.com/ews/exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (exchangeUrl.Contains("example.com") ||
                    username.Contains("example.com") ||
                    password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create and connect the Exchange WebDAV client.
                using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
                {
                    try
                    {
                        // Retrieve the top‑level public folders.
                        ExchangeFolderInfoCollection publicFolders = client.ListPublicFolders();

                        foreach (ExchangeFolderInfo folder in publicFolders)
                        {
                            PrintFolderHierarchy(client, folder, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error accessing Exchange server: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Recursively prints a folder and its subfolders with indentation.
        private static void PrintFolderHierarchy(ExchangeClient client, ExchangeFolderInfo folder, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2);
            Console.WriteLine($"{indent}- {folder.DisplayName}");

            try
            {
                ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folder);
                foreach (ExchangeFolderInfo subFolder in subFolders)
                {
                    PrintFolderHierarchy(client, subFolder, indentLevel + 1);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{indent}  (Failed to list subfolders: {ex.Message})");
            }
        }
    }
}
