using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (serviceUrl.Contains("example") || username.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping DeleteFolder operation.");
                return;
            }

            // Create the Exchange Web Services client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new System.Net.NetworkCredential(username, password)))
            {
                string folderUri = "/Inbox/TargetFolder";

                try
                {
                    // Delete the folder permanently
                    client.DeleteFolder(folderUri, true);
                    Console.WriteLine($"Folder '{folderUri}' deleted permanently.");
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
