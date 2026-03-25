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
            // Initialize token provider for Outlook (as a placeholder for authentication)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                clientId: "your-client-id",
                clientSecret: "your-client-secret",
                refreshToken: "your-refresh-token");

            // Service URL for the Zimbra activity endpoint (replace with actual URL)
            string serviceUrl = "https://zimbra.example.com/activity";

            // Create Activity client
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure webhook for receiving notifications
                var webhook = new Webhook
                {
                    Address = "https://yourapp.example.com/webhook/receive",
                    Expiration = DateTime.UtcNow.AddDays(30)
                };

                // Content type for the subscription payload
                string contentType = "application/json";

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