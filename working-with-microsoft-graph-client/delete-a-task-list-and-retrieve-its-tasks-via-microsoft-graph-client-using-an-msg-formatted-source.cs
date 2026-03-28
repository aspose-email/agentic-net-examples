using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare MSG file path
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(msgPath, SaveOptions.DefaultMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Use the loaded message as needed (e.g., extract subject)
            Console.WriteLine($"Loaded message subject: {message.Subject}");

            // Prepare token provider (Outlook) with dummy credentials
            TokenProvider tokenProvider;
            try
            {
                tokenProvider = TokenProvider.Outlook.GetInstance("clientId", "clientSecret", "refreshToken");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            // Initialize Graph client (requires token provider and tenant ID)
            IGraphClient client;
            try
            {
                client = GraphClient.GetClient(tokenProvider, "tenantId");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (client)
            {
                // Define the Task List ID to delete and later retrieve tasks from
                string taskListId = "tasklist-id";

                // Delete the specified Task List
                try
                {
                    client.DeleteTaskList(taskListId);
                    Console.WriteLine($"Deleted Task List with ID: {taskListId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete Task List: {ex.Message}");
                    // Continue to attempt retrieval (may be empty)
                }

                // Retrieve tasks from the (now deleted) Task List
                try
                {
                    var tasks = client.ListTasks(taskListId);
                    Console.WriteLine($"Tasks in Task List '{taskListId}':");
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"- {task.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list tasks: {ex.Message}");
                }
            }

            // Dispose the loaded message
            message.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
