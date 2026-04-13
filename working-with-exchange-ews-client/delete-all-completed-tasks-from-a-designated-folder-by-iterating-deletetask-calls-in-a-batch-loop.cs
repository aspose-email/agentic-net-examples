using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // EWS connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve all tasks from the default Tasks folder
                TaskCollection tasks = client.ListTasks();

                // Iterate through tasks and delete those that are completed
                foreach (ExchangeTask task in tasks)
                {
                    if (task.Status == ExchangeTaskStatus.Completed)
                    {
                        try
                        {
                            // Delete using the task's UniqueId (not UniqueUri)
                            client.DeleteItem(task.UniqueId, new DeletionOptions(DeletionType.MoveToDeletedItems));
                            Console.WriteLine($"Deleted task: {task.Subject}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete task '{task.Subject}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
