using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

namespace AsposeEmailGraphSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials and identifiers
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";
                string taskListId = "your-tasklist-id";

                // Skip execution if placeholders are not replaced
                if (clientId.StartsWith("your-") ||
                    clientSecret.StartsWith("your-") ||
                    refreshToken.StartsWith("your-") ||
                    tenantId.StartsWith("your-") ||
                    taskListId.StartsWith("your-"))
                {
                    Console.Error.WriteLine("Please replace placeholder credentials and identifiers with real values.");
                    return;
                }

                // Create token provider (3‑argument overload)
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize Graph client (Aspose.Email.Clients.ITokenProvider overload)
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    try
                    {
                        // Delete the specified task list
                        client.DeleteTaskList(taskListId);
                        Console.WriteLine($"Task list '{taskListId}' deleted successfully.");

                        // Retrieve tasks from the (now deleted) task list – this will typically return empty collection
                        MapiTaskCollection tasks = client.ListTasks(taskListId);
                        Console.WriteLine($"Tasks in task list '{taskListId}':");

                        foreach (MapiTask task in tasks)
                        {
                            Console.WriteLine($"- Subject: {task.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
