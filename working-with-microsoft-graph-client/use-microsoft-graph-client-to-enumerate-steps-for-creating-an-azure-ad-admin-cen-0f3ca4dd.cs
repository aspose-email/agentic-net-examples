using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage object
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Prepare token provider for Microsoft Graph (Outlook)
                // Replace the placeholder values with real credentials
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";

                // Create the token provider instance
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize the Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Example: List the user's mail folders
                    var folders = graphClient.ListFolders();
                    Console.WriteLine("User's mail folders:");
                    foreach (var folder in folders)
                    {
                        Console.WriteLine($"- {folder.DisplayName} (Id: {folder.ItemId})");
                    }

                    // Choose a target folder (e.g., Drafts) by name
                    string targetFolderId = null;
                    foreach (var folder in folders)
                    {
                        if (string.Equals(folder.DisplayName, "Drafts", StringComparison.OrdinalIgnoreCase))
                        {
                            targetFolderId = folder.ItemId;
                            break;
                        }
                    }

                    if (targetFolderId == null)
                    {
                        Console.Error.WriteLine("Drafts folder not found.");
                        return;
                    }

                    // Create a new message in the target folder using the loaded MSG content
                    MailMessage createdMessage = graphClient.CreateMessage(targetFolderId, mailMessage);
                    Console.WriteLine($"Message created with Subject: {createdMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}