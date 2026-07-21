using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphSample
{
    class Program
    {
        static void Main()
        {
            // Configuration – replace with your actual Azure AD app details and message ID.
            string clientId = "your-client-id";
            string tenantId = "your-tenant-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string messageId = "target-message-id";

            // Output file for the fetched MSG message.
            string outputPath = "FetchedMessage.msg";

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Acquire an OAuth token provider for Microsoft Graph.
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Create the Graph client.
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Fetch the message as a MapiMessage.
                    using (MapiMessage message = graphClient.FetchMessage(messageId))
                    {
                        // Display basic information.
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.SenderEmailAddress}");
                        Console.WriteLine($"Received: {message.ClientSubmitTime}");

                        // Save the message in MSG format.
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
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
