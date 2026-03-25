using System;
using System.Net;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize token provider for Outlook (replace with actual credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "your-client-id",
                "your-client-secret",
                "your-refresh-token");

            // Service URL for the Activity (Exchange) endpoint (replace with actual URL)
            string serviceUrl = "https://outlook.office365.com";

            // Create Activity client
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure webhook for subscription notifications
                Webhook webhook = new Webhook();
                webhook.Address = "https://yourapp.example.com/webhook/receive";
                webhook.Expiration = DateTime.UtcNow.AddDays(30);

                // Content type representing the Zimbra product update channel
                string contentType = "productUpdates";

                // Start subscription
                activityClient.StartSubscription(contentType, webhook);
                Console.WriteLine("Subscription to product update notifications has been enabled.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}