using System;
using System.Net;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;


class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider for Outlook (used as an example for OAuth token retrieval)
            // Replace with actual clientId, clientSecret, and refreshToken for your Zimbra integration
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                clientId: "YOUR_CLIENT_ID",
                clientSecret: "YOUR_CLIENT_SECRET",
                refreshToken: "YOUR_REFRESH_TOKEN");

            // Create an Activity client instance (used for subscription services)
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, "https://zimbra.example.com/api"))
            {
                // Configure webhook details for receiving notifications
                var webhook = new Webhook
                {
                    Address = "https://yourapp.example.com/webhook/receive",
                    Expiration = DateTime.UtcNow.AddDays(30)
                };

                // Start subscription for product update notifications (content type can be adjusted as needed)
                activityClient.StartSubscription(contentType: "productUpdates", webhook: webhook);
                Console.WriteLine("Subscription to Zimbra product updates has been enabled.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}