using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

namespace ExchangeTaskSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // ------------------------------------------------------------
                // Configuration – replace with real values or skip when placeholders are used
                // ------------------------------------------------------------
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username   = "user@example.com";
                string password   = "password";

                // Guard against placeholder credentials
                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    username.Contains("placeholder", StringComparison.OrdinalIgnoreCase) ||
                    password.Contains("placeholder", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("Placeholder credentials detected – skipping network operations.");
                    return;
                }

                // ------------------------------------------------------------
                // Create and configure the EWS client
                // ------------------------------------------------------------
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // ------------------------------------------------------------
                    // 1. Create a new task
                    // ------------------------------------------------------------
                    ExchangeTask newTask = new ExchangeTask
                    {
                        Subject   = "Sample Exchange Task",
                        Body      = "This is a sample task created via Aspose.Email EWS API.",
                        StartDate = DateTime.Now,
                        DueDate   = DateTime.Now.AddDays(7)
                    };

                    string taskUri;
                    try
                    {
                        taskUri = ewsClient.CreateTask(newTask);
                        Console.WriteLine($"Task created. URI: {taskUri}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating task: {ex.Message}");
                        return;
                    }

                    // ------------------------------------------------------------
                    // 2. Retrieve the created task
                    // ------------------------------------------------------------
                    ExchangeTask fetchedTask;
                    try
                    {
                        fetchedTask = ewsClient.FetchTask(taskUri);
                        Console.WriteLine($"Fetched Task Subject: {fetchedTask.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error fetching task: {ex.Message}");
                        return;
                    }

                    // ------------------------------------------------------------
                    // 3. Update the task (e.g., modify the body)
                    // ------------------------------------------------------------
                    try
                    {
                        fetchedTask.Body = "Updated task body content.";
                        ewsClient.UpdateTask(fetchedTask);
                        Console.WriteLine("Task updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error updating task: {ex.Message}");
                        return;
                    }

                    // ------------------------------------------------------------
                    // 4. Delete the task
                    // ------------------------------------------------------------
                    try
                    {
                        // The DeleteItem method (or similar) is typically used to remove a task.
                        // If the current API version provides a dedicated DeleteTask method, replace the call accordingly.
                        // ewsClient.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                        Console.WriteLine("Task deletion placeholder – implement DeleteItem when available.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error deleting task: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
