using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the EWS client with placeholder credentials
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                "username",
                "password"))
            {
                // Create and configure a new Exchange task
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Sample Task";
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(2);
                task.Body = "Complete the sample task.";

                // Create the task on the server
                client.CreateTask(task);
                Console.WriteLine("Task created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
