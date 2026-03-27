using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file that contains task list information
            string msgPath = "tasklist.msg";

            // Verify the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file (as a MailMessage) inside a using block for disposal
            using (MailMessage msg = MailMessage.Load(msgPath))
            {
                // Placeholder: extract task list identifier from the message if needed
                // For this example we use a hard‑coded task list id
                string taskListId = "YOUR_TASK_LIST_ID";

                // Create a token provider for Outlook (Microsoft Graph) authentication
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Initialize the Graph client using the token provider
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    try
                    {
                        // Delete the specified task list
                        client.DeleteTaskList(taskListId);
                        Console.WriteLine($"Deleted task list with id: {taskListId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error deleting task list: {ex.Message}");
                        // Continue to attempt retrieval of tasks (if any)
                    }

                    try
                    {
                        // Retrieve tasks from the (now deleted) task list
                        MapiTaskCollection tasks = client.ListTasks(taskListId);
                        Console.WriteLine($"Tasks in task list {taskListId}:");

                        foreach (MapiTask task in tasks)
                        {
                            Console.WriteLine($"- {task.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error listing tasks: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
