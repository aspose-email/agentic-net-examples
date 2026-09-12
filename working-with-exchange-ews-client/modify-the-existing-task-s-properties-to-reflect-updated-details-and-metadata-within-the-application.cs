using Aspose.Email.Mapi;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Exchange server connection details
        string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";


        // Skip external calls when placeholder credentials are used
        if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Initialize the EWS client
            IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password);

            // Create a new task
            ExchangeTask newTask = new ExchangeTask();
            newTask.Subject = "Sample Task";
            newTask.Body = "This is a sample task created via Aspose.Email.";
            newTask.StartDate = DateTime.Now;
            newTask.DueDate = DateTime.Now.AddDays(7);

            // Create the task in the default task folder; returns the task URI
            string taskUri = client.CreateTask(newTask);
            Console.WriteLine($"Task created. URI: {taskUri}");

            // Prepare updated task information using MapiTask (overload that accepts URI + MapiTask)
            MapiTask updatedTask = new MapiTask();
            updatedTask.Subject = "Updated Sample Task";
            updatedTask.Body = "This is the updated body of the task.";
            updatedTask.StartDate = DateTime.Now;
            updatedTask.DueDate = DateTime.Now.AddDays(10);

            // Update the existing task
            string updatedTaskUri = client.UpdateTask(taskUri, updatedTask);
            Console.WriteLine($"Task updated. New URI: {updatedTaskUri}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
