using System;
using System.Net;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider (replace with real credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken"
            );

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                try
                {
                    // Prepare a new task list
                    TaskListInfo newList = new TaskListInfo();
                    // Set required properties (e.g., display name) – adjust as per actual API
                    // newList.DisplayName = "My Sample Task List";

                    // Create the task list on the server
                    TaskListInfo createdList = client.CreateTaskList(newList);

                    Console.WriteLine("Task list created successfully.");
                    // Optionally output identifier
                    // Console.WriteLine("List ID: " + createdList.Id);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during Graph operation: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}