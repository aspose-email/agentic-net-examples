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
            // Placeholder credentials and service URL
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials/hosts to avoid real network calls
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder service URL detected. Skipping network operations.");
                return;
            }

            // Create the EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (client as IDisposable)
            {
                // Instantiate a folder info object (using ExchangeFolderUserInfo which allows setting DisplayName)
                ExchangeFolderUserInfo folderInfo = new ExchangeFolderUserInfo();
                folderInfo.DisplayName = "My Custom Folder";

                Console.WriteLine($"Folder display name set to: {folderInfo.DisplayName}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
