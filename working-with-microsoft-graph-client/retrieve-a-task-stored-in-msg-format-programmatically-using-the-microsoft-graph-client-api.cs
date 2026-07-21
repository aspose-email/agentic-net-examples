using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphTaskExample
{
    // Author: Aspose.Email example for retrieving a task via Microsoft Graph and saving as MSG.
    class Program
    {
        static void Main()
        {
            // Replace the placeholder values with your actual Azure app credentials and task ID.
            string requestUrl = "YOUR_REQUEST_URL";
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string tenantId = "YOUR_TENANT_ID";
            string taskId = "YOUR_TASK_ID";

            // Guard to ensure placeholders are replaced.
            if (requestUrl.StartsWith("YOUR_") ||
                clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") ||
                tenantId.StartsWith("YOUR_") ||
                taskId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace all placeholder values (YOUR_*) with actual data.");
                return;
            }

            try
            {
                // Obtain an OAuth token provider.
                using (TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken))
                {
                    // Initialize the Graph client.
                    using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                    {
                        // Retrieve the task from Microsoft Graph.
                        MapiTask task = graphClient.FetchTask(taskId);

                        // Convert the task to its underlying MAPI message.
                        MapiMessage message = task.GetUnderlyingMessage();

                        // Determine a safe output location.
                        string outputDirectory = Path.GetTempPath();
                        string outputPath = Path.Combine(outputDirectory, "retrievedTask.msg");

                        // Ensure the directory exists.
                        if (!Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        // Save the task as an MSG file.
                        message.Save(outputPath);
                        Console.WriteLine($"Task saved as MSG to: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
