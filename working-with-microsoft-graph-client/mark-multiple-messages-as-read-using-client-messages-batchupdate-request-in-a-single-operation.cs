using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string tenantId = "YOUR_TENANT_ID";

            // Guard against placeholder credentials to avoid real network calls in CI.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") || tenantId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph client operations.");
                return;
            }

            // Create token provider.
            var tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, tenantId);

            // Initialize Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Example list of message IDs to mark as read.
                List<string> messageIds = new List<string>
                {
                    "AAMkAGI2.../Mail.ReadWrite",
                    "AAMkAGI2.../Mail.ReadWrite2"
                };

                // Mark each message as read. If a BatchUpdate method exists, it can replace this loop.
                foreach (string id in messageIds)
                {
                    try
                    {
                        client.SetRead(id);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to mark message {id} as read: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
