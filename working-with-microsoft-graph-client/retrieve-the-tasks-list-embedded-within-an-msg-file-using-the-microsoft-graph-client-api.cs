using System;
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
            // Initialize token provider (replace with real credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                // Retrieve all task lists
                TaskListInfoCollection taskLists = client.ListTaskLists();

                foreach (TaskListInfo taskListInfo in taskLists)
                {
                    Console.WriteLine($"Task List: {taskListInfo.DisplayName}");

                    // Retrieve tasks from the current task list
                    MapiTaskCollection tasks = client.ListTasks(taskListInfo.ItemId);

                    foreach (MapiTask task in tasks)
                    {
                        Console.WriteLine($"- Subject: {task.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
