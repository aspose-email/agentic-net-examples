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
            // Path to the MSG file containing the task details
            string msgPath = "task.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine("The MSG file was not found: " + msgPath);
                return;
            }

            // Token provider credentials (replace with real values)
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Create the token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize the Graph client
            using (IGraphClient graphClient = Aspose.Email.Clients.Graph.GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Retrieve the first task list (default task list)
                var taskLists = graphClient.ListTaskLists();
                if (taskLists == null || taskLists.Count == 0)
                {
                    Console.Error.WriteLine("No task lists were found in the mailbox.");
                    return;
                }

                string taskListId = taskLists[0].ItemId;

                // Load the MSG file as a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                {
                    // Create a MapiTask and populate basic fields
                    using (MapiTask task = new MapiTask())
                    {
                        task.Subject = mapiMessage.Subject;
                        task.Body = mapiMessage.Body;

                        // Create the task in the specified task list via Graph
                        MapiTask createdTask = graphClient.CreateTask(task, taskListId);
                        Console.WriteLine("Task created successfully. Subject: " + createdTask.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}