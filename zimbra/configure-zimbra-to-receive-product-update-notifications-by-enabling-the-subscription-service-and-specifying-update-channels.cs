using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider (replace with actual credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                clientId: "your-client-id",
                clientSecret: "your-client-secret",
                refreshToken: "your-refresh-token");

            // Service URL for Zimbra activity (replace with actual endpoint)
            string serviceUrl = "https://zimbra.example.com/api/activity";

            // Create Activity client
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure webhook for receiving notifications
                Webhook webhook = new Webhook
                {
                    Address = "https://yourapp.example.com/webhook/receive",
                    Expiration = DateTime.UtcNow.AddDays(30)
                };

                // Define the content type (update channel) to subscribe to
                string contentType = "product-updates";

                // Start the subscription
                activityClient.StartSubscription(contentType, webhook);
                Console.WriteLine("Subscription to product update notifications started successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}