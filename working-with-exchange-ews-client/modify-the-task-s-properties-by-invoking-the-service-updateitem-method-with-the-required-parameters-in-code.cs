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
            // Service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Create a new task
                ExchangeTask newTask = new ExchangeTask();
                newTask.Subject = "Sample Task";
                newTask.Body = "This is a sample task created via Aspose.Email.";
                newTask.DueDate = DateTime.Now.AddDays(2);
                newTask.Status = ExchangeTaskStatus.NotStarted;

                // Add the task to the server and obtain its URI
                string taskUri = client.CreateTask(newTask);
                Console.WriteLine("Task created with URI: " + taskUri);

                // Retrieve the task from the server
                ExchangeTask fetchedTask = client.FetchTask(taskUri);

                // Modify the task's status
                fetchedTask.Status = ExchangeTaskStatus.Completed;

                // Update the task on the server
                client.UpdateTask(fetchedTask);
                Console.WriteLine("Task status updated to Completed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
