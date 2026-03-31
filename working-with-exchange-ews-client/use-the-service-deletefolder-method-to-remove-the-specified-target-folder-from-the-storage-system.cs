using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Skipping execution because placeholder credentials are used.");
                return;
            }

            // Target folder URI to delete (example: Inbox subfolder)
            string folderUri = "https://exchange.example.com/EWS/Exchange.asmx/Inbox/TargetFolder";

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Delete the specified folder
                    client.DeleteFolder(folderUri);
                    Console.WriteLine($"Folder '{folderUri}' deleted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle errors related to the DeleteFolder operation
                    Console.Error.WriteLine($"Error deleting folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
