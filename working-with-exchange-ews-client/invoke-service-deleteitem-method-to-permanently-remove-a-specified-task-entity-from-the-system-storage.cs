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
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");
            using (IEWSClient service = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // URI of the task to be deleted
                string taskUri = "task-uri-placeholder";

                // Permanently delete the specified task
                service.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                Console.WriteLine("Task deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
