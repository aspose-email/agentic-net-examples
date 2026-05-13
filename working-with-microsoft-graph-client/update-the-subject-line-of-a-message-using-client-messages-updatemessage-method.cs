using Aspose.Email.Mapi;
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
            // Placeholder credentials and message identifier
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string tenantId = "your-tenant-id";
            string refreshToken = "your-refresh-token";
            string messageId = "your-message-id";

            // Skip execution when placeholders are present
            if (clientId.StartsWith("your-") ||
                clientSecret.StartsWith("your-") ||
                tenantId.StartsWith("your-") ||
                refreshToken.StartsWith("your-") ||
                messageId.StartsWith("your-"))
            {
                Console.WriteLine("Placeholder values detected. Skipping Graph operations.");
                return;
            }

            // Create token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
            {
                try
                {
                    // Fetch the message to be updated
                    using (MapiMessage message = client.FetchMessage(messageId))
                    {
                        // Update the subject line
                        message.Subject = "Updated Subject";

                        // Send the update request
                        client.UpdateMessage(message);
                    }

                    Console.WriteLine("Message subject updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during message update: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
