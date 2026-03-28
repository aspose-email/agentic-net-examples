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
            // Create token provider for Outlook (Microsoft Graph)
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            string tenantId = "yourTenantId";

            // Initialize Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // ID of the MSG message to retrieve
                string messageId = "message-id";

                // Fetch the message as a MapiMessage
                using (MapiMessage msg = client.FetchMessage(messageId))
                {
                    // Define output path
                    string outputPath = "output.msg";
                    string directory = Path.GetDirectoryName(outputPath);

                    // Ensure the directory exists
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the message in MSG format
                    msg.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
