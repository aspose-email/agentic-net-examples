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
            // Path to the MSG file that contains the task list identifier
            string msgFilePath = "taskmsg.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Load the MSG file as a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(msgFilePath))
            {
                // Attempt to retrieve a custom header that holds the Task List ID
                // (In a real scenario the MSG file should contain this identifier)
                string taskListId = mailMessage.Headers["TaskListId"];
                if (string.IsNullOrEmpty(taskListId))
                {
                    Console.Error.WriteLine("TaskListId header not found in the MSG file.");
                    return;
                }

                // Create a token provider for Microsoft Graph authentication
                // Replace the placeholder strings with actual credentials
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Initialize the Graph client using the token provider and tenant ID
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Retrieve the tasks from the specified task list
                    MapiTaskCollection tasks = graphClient.ListTasks(taskListId);

                    // Output basic information about each task
                    foreach (MapiTask task in tasks)
                    {
                        Console.WriteLine($"Subject: {task.Subject}");
                        Console.WriteLine($"Start Date: {task.StartDate}");
                        Console.WriteLine($"Due Date: {task.DueDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}