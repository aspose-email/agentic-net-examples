using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider with placeholder credentials
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.GetInstance(
                "https://login.microsoftonline.com/common/oauth2/v2.0/token",
                "clientId",
                "clientSecret",
                "refreshToken");

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // IDs of the folder to move and the target parent folder
                string folderId = "sourceFolderId";
                string newParentFolderId = "destinationParentFolderId";

                // Move the folder
                FolderInfo movedFolder = client.MoveFolder(newParentFolderId, folderId);
                Console.WriteLine("Folder moved successfully. New ID: " + movedFolder.ItemId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
