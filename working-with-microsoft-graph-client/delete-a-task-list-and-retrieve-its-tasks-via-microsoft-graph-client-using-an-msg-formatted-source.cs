using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file that contains the TaskList ID
            string msgPath = "tasklist.msg";

            // Verify that the file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage (disposable)
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Assume the TaskList ID is stored in the message body
                string taskListId = mailMessage.Body.Trim();
                if (string.IsNullOrEmpty(taskListId))
                {
                    Console.Error.WriteLine("Task list ID not found in the message body.");
                    return;
                }

                // Create a token provider (Outlook) with dummy credentials
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Initialize the Graph client (disposable)
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Delete the specified TaskList
                    try
                    {
                        graphClient.DeleteTaskList(taskListId);
                        Console.WriteLine($"Deleted TaskList with ID: {taskListId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete TaskList: {ex.Message}");
                    }

                    // Retrieve tasks from the (now deleted) TaskList
                    try
                    {
                        MapiTaskCollection tasks = graphClient.ListTasks(taskListId);
                        Console.WriteLine($"Tasks in TaskList {taskListId}:");
                        foreach (MapiTask task in tasks)
                        {
                            Console.WriteLine($"- {task.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list tasks: {ex.Message}");
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