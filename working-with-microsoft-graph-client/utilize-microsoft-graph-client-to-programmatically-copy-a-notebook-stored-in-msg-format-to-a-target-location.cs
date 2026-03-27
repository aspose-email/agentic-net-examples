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
            // Paths and credentials (replace with actual values)
            string msgFilePath = "notebook.msg";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage notebookMessage = MapiMessage.Load(msgFilePath))
            {
                // Placeholder notebook ID (in a real scenario, obtain the actual ID)
                string notebookItemId = "placeholder-notebook-id";

                // Target group ID where the notebook will be copied (optional)
                string targetGroupId = "target-group-id";

                // Optional new name for the copied notebook
                string newNotebookName = "Copied Notebook";

                // Create token provider (Outlook token provider expects three arguments)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize the Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Initiate the copy operation
                    string operationLocation = graphClient.CopyNotebook(notebookItemId, targetGroupId, newNotebookName);
                    Console.WriteLine("Copy operation initiated. Operation-Location: " + operationLocation);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}