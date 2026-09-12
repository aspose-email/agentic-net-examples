using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings (replace with real values)
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";
            string domain = "example.com";

            // Name of the folder to create
            string folderName = "NewFolder";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password, domain))
            {
                // Create the folder in the mailbox root
                client.CreateFolder(folderName);
                Console.WriteLine($"Folder '{folderName}' created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
