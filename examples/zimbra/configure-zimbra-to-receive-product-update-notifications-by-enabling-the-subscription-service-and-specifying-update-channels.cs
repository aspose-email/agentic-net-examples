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
            // Placeholder credentials for the token provider
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Create an Outlook token provider (used here as an example)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize the Activity client for Zimbra (service URL is illustrative)
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, "https://activity.zimbra.com"))
            {
                // Configure the webhook that will receive product update notifications
                Webhook webhook = new Webhook();
                webhook.Address = "https://yourapp.example.com/webhook/receive";
                webhook.Expiration = DateTime.UtcNow.AddDays(30);

                // Enable the subscription service for product updates
                string contentType = "application/json";
                activityClient.StartSubscription(contentType, webhook);

                Console.WriteLine("Subscription service enabled and update channels configured.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}