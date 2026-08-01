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
            // ======== Placeholder values (replace with real credentials) ========
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string tenantId = "YOUR_TENANT_ID";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string graphEndpoint = "YOUR_GRAPH_ENDPOINT"; // e.g., "https://graph.microsoft.com/v1.0"
            string messageId = "YOUR_MESSAGE_ID";
            string attachmentId = "YOUR_ATTACHMENT_ID";
            string outputPath = "YOUR_OUTPUT_PATH.msg";
            // ====================================================================

            // Simple guard for placeholder literals
            if (string.IsNullOrWhiteSpace(clientId) || !clientId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(clientSecret) || !clientSecret.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(tenantId) || !tenantId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(refreshToken) || !refreshToken.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(graphEndpoint) || !graphEndpoint.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(messageId) || !messageId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(attachmentId) || !attachmentId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(outputPath) || !outputPath.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("One or more placeholder values are not set. Please replace the \"YOUR_...\" strings with actual values.");
                return;
            }

            // Ensure output directory exists
            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Authenticate and obtain a Graph client
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, graphEndpoint))
            {
                try
                {
                    // Fetch the attachment by its id
                    MapiAttachment attachment = graphClient.FetchAttachment(attachmentId);
                    if (attachment == null)
                    {
                        Console.Error.WriteLine("Attachment not found.");
                        return;
                    }

                    // Save the attachment as an MSG file
                    attachment.Save(outputPath);
                    Console.WriteLine($"Attachment saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Graph operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
