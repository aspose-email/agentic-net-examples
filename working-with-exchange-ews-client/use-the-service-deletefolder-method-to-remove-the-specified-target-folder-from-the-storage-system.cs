using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Service URL and credentials (replace with real values as needed)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // URI of the folder to be deleted
                string folderUri = "https://exchange.example.com/EWS/Exchange.asmx/Inbox/TargetFolder";

                // Delete the folder
                client.DeleteFolder(folderUri);
                Console.WriteLine("Folder deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
