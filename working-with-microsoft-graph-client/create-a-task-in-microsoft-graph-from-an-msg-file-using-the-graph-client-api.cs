using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Create a token provider for Outlook (3‑argument overload)
                Aspose.Email.Clients.TokenProvider tokenProvider;
                try
                {
                    tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                        "clientId",          // Replace with your client ID
                        "clientSecret",      // Replace with your client secret
                        "refreshToken");     // Replace with your refresh token
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                    return;
                }

                // Tenant identifier for Microsoft Graph
                string tenantId = "your-tenant-id"; // Replace with your tenant ID

                // Initialize the Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Convert the MailMessage to a MapiTask (simple mapping)
                    MapiTask task = new MapiTask
                    {
                        Subject = message.Subject,
                        Body = message.Body
                    };

                    // Folder identifier where the task will be created.
                    // For demonstration, using the default "Tasks" folder name.
                    string tasksFolderId = "Tasks";

                    // Create the task in Microsoft Graph
                    client.CreateTask(task, tasksFolderId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
