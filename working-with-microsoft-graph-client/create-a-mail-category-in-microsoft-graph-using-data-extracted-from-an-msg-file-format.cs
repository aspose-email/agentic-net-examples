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
            // Input MSG file path
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Extract the first category name from the message (if any)
                string categoryName = "DefaultCategory";
                if (msg.Categories != null && msg.Categories.Length > 0)
                {
                    categoryName = msg.Categories[0];
                }

                // Prepare Graph authentication (dummy credentials for illustration)
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";

                // Obtain a token provider instance
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    clientId, clientSecret, refreshToken);

                // Create the Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Create a new Outlook category using a preset color
                    OutlookCategory createdCategory = graphClient.CreateCategory(
                        categoryName,
                        CategoryPreset.Preset0); // Use any valid preset enum value

                    Console.WriteLine($"Category created: {createdCategory.DisplayName}, Preset: {createdCategory.Preset}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}