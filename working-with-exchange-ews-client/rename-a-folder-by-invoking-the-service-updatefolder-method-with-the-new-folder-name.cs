using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider (replace with a real implementation)
            Aspose.Email.Clients.ITokenProvider tokenProvider = null; // placeholder
            string tenantId = "your-tenant-id";

            // Create the Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Identify the folder to rename (replace with actual folder ID)
                string folderId = "folder-id-to-rename";

                // Retrieve the folder information
                FolderInfo folder = client.GetFolder(folderId);

                // Set the new display name
                folder.DisplayName = "New Folder Name";

                // Update the folder on the server
                FolderInfo updatedFolder = client.UpdateFolder(folder);

                Console.WriteLine("Folder renamed to: " + updatedFolder.DisplayName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
