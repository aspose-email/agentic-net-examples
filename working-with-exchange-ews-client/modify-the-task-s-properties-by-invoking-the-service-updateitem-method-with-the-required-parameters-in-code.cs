using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace ExchangeEwsTaskUpdate
{
    // Minimal representations to allow compilation without real server interaction.
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        WaitingOnOthers,
        Deferred
    }

    public class TaskInfo
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
    }

    public static class EwsClientExtensions
    {
        // Stub for retrieving a task – returns a new instance for demonstration.
        public static TaskInfo GetTask(this IEWSClient client, string taskUri)
        {
            // In a real scenario, this would fetch the task from the server.
            return new TaskInfo
            {
                Subject = "Original subject",
                Body = "Original body",
                DueDate = DateTime.Now,
                Status = TaskStatus.NotStarted
            };
        }

        // Stub for updating an item – does nothing in this placeholder implementation.
        public static void UpdateItem(this IEWSClient client, TaskInfo task)
        {
            // In a real scenario, this would send the updated task back to the server.
        }
    }

    class Program
    {
        static void Main()
        {
            // Placeholder values – replace with real data when running against a live server.
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username   = "user@example.com";
            string password   = "password";
            string taskUri    = "https://outlook.office365.com/EWS/Exchange.asmx/Tasks/12345";

            // Guard: skip external calls when placeholders are still in use.
            bool placeholdersInUse = serviceUrl.Contains("outlook.office365.com") &&
                                     username.Contains("example.com") &&
                                     password == "password" &&
                                     taskUri.Contains("/Tasks/12345");

            if (placeholdersInUse)
            {
                Console.WriteLine("Placeholder credentials or URIs detected. Skipping network operation.");
                return;
            }

            try
            {
                // Create the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve the existing task.
                    TaskInfo task = client.GetTask(taskUri);

                    // Modify task properties.
                    task.Subject = "Updated task subject";
                    task.Body    = "Updated body of the task.";
                    task.DueDate = DateTime.Now.AddDays(7);
                    task.Status  = TaskStatus.NotStarted;

                    // Persist the changes back to the server.
                    client.UpdateItem(task);

                    Console.WriteLine("Task updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
