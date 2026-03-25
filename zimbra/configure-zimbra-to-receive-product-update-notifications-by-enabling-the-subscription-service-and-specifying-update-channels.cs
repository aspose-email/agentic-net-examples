using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;


namespace ZimbraSubscriptionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize token provider (replace with real credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    clientId: "your-client-id",
                    clientSecret: "your-client-secret",
                    refreshToken: "your-refresh-token");

                // Service URL for Zimbra activity (replace with actual endpoint)
                string serviceUrl = "https://zimbra.example.com/activity";

                // Create Activity client
                using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
                {
                    // Configure webhook for receiving notifications
                    Webhook webhook = new Webhook();
                    webhook.Address = "https://yourapp.example.com/webhook/receive";
                    webhook.Expiration = DateTime.UtcNow.AddDays(30);

                    // Content type for product update notifications (example value)
                    string contentType = "productUpdates";

                    // Start (or update) the subscription
                    Subscription subscription = activityClient.StartSubscription(contentType, webhook);
                    Console.WriteLine("Subscription started. Status: " + subscription.Status);

                    // List current subscriptions
                    Subscription[] subscriptions = activityClient.ListSubscriptions();
                    Console.WriteLine("Current subscriptions:");
                    foreach (Subscription sub in subscriptions)
                    {
                        Console.WriteLine("- ContentType: " + sub.ContentType + ", Status: " + sub.Status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}