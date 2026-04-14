using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace ListTasksSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connection parameters
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Folder ID containing the tasks
                string folderId = "tasksFolderId";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve tasks from the specified folder
                    TaskCollection tasks = client.ListTasks(folderId);

                    // Output each task's subject and status
                    foreach (ExchangeTask task in tasks)
                    {
                        Console.WriteLine($"Subject: {task.Subject}, Status: {task.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
