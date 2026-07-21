using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service endpoint and credentials
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client (implements IEWSClient)
            using (IEWSClient service = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // URI of the task to be deleted
                string taskItemUri = "https://outlook.office365.com/EWS/Exchange.asmx/Tasks/12345";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Permanently delete the task
                service.DeleteItem(taskItemUri, DeletionOptions.DeletePermanently);

                Console.WriteLine("Task deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
