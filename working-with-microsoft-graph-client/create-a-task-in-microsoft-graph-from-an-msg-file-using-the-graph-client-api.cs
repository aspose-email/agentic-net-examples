using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Input parameters (placeholders)
            string msgFilePath = "task.msg";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";
            string taskFolderId = "tasks";

            // Guard against placeholder credentials to avoid real network calls
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-") || tenantId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operation.");
                return;
            }

            // Ensure the MSG file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Create a simple MapiTask from the loaded message (using subject and body)
            MapiTask task = new MapiTask
            {
                Subject = msg.Subject,
                Body = msg.Body,
                // Set minimal required properties; dates can be set to now
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };

            // Initialize the token provider for Outlook
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Create the Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                try
                {
                    // Create the task in the specified folder
                    MapiTask createdTask = client.CreateTask(task, taskFolderId);
                    Console.WriteLine($"Task created with Subject: {createdTask.Subject}");
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
