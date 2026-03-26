using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;

class Program
{
    static void Main()
    {
        try
        {
            // Create token provider for Outlook (replace placeholders with real values)
            TokenProvider outlookTokenProvider = TokenProvider.Outlook.GetInstance(
                "your-client-id",
                "your-client-secret",
                "your-refresh-token");

            // Initialize Activity client (tenant identifier placeholder)
            using (IActivityClient activityClient = ActivityClient.GetClient(outlookTokenProvider, "your-tenant-id"))
            {
                // Define the content type to subscribe to (e.g., product update notifications)
                string contentType = "productUpdates";

                // Configure webhook endpoint and expiration
                Webhook webhook = new Webhook();
                webhook.Address = "https://yourapp.example.com/webhook/receive";
                webhook.Expiration = DateTime.UtcNow.AddDays(30);

                // Start the subscription
                activityClient.StartSubscription(contentType, webhook);
                Console.WriteLine("Subscription started successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}