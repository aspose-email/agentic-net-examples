using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

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
            string domain = ""; // optional, leave empty if not required

            // Unique identifier (URI) of the task to delete
            string taskUri = "task-unique-uri-or-id";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password, domain))
            {
                // Delete the task by its URI, moving it to Deleted Items
                client.DeleteItem(taskUri, new DeletionOptions(DeletionType.MoveToDeletedItems));

                Console.WriteLine("Task deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
