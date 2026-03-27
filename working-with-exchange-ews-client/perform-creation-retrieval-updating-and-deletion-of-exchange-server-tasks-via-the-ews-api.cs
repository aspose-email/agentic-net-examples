using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // EWS service URL and credentials
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                // -------------------- Create a new task --------------------
                ExchangeTask newTask = new ExchangeTask();
                newTask.Subject = "Sample Task";
                newTask.Body = "This is a sample task created via EWS.";
                newTask.StartDate = DateTime.Now;
                newTask.DueDate = DateTime.Now.AddDays(7);
                // Create the task in the default Tasks folder
                string taskUri = client.CreateTask(newTask);
                Console.WriteLine("Task created. URI: " + taskUri);

                // -------------------- Retrieve the task --------------------
                ExchangeTask fetchedTask = client.FetchTask(taskUri);
                Console.WriteLine("Fetched Task Subject: " + fetchedTask.Subject);
                Console.WriteLine("Fetched Task Body: " + fetchedTask.Body);
                Console.WriteLine("Fetched Task DueDate: " + fetchedTask.DueDate);

                // -------------------- Update the task --------------------
                fetchedTask.Body = "Updated task body content.";
                client.UpdateTask(fetchedTask);
                Console.WriteLine("Task updated.");

                // -------------------- Delete the task --------------------
                client.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                Console.WriteLine("Task deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}