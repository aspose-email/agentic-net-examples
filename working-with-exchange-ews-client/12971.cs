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
            // Placeholder connection details – real values should be provided in production.
            string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected to avoid unwanted network calls.
            if (serverUrl.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create and authenticate the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serverUrl, new NetworkCredential(username, password)))
            {
                // -------------------- Create a new task --------------------
                ExchangeTask newTask = new ExchangeTask();
                newTask.Subject = "Sample Task";
                newTask.Body = "This is a sample task created via Aspose.Email.";
                newTask.DueDate = DateTime.Now.AddDays(7);

                // CreateTask returns the task URI.
                string taskUri = client.CreateTask(newTask);
                Console.WriteLine($"Task created. Uri: {taskUri}");

                // -------------------- Retrieve the task --------------------
                // FetchTask expects a task URI; after fetching we obtain the UniqueId.
                ExchangeTask fetchedTask = client.FetchTask(taskUri);
                string uniqueId = fetchedTask.UniqueId;
                Console.WriteLine($"Fetched task UniqueId: {uniqueId}");

                // -------------------- Update the task --------------------
                fetchedTask.Body = "Updated body content.";
                client.UpdateTask(fetchedTask);
                Console.WriteLine("Task updated.");

                // -------------------- Delete the task --------------------
                // Deletion must use the UniqueId, not the UniqueUri.
                DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                client.DeleteItem(uniqueId, deleteOptions);
                Console.WriteLine("Task deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
