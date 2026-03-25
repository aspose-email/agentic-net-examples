using System;
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
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Activity service URL for Zimbra (replace with actual URL)
            string serviceUrl = "https://zimbra.example.com/activity";

            // Create Activity client
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure webhook for notifications
                Webhook webhook = new Webhook();
                webhook.Address = "https://yourapp.example.com/webhook/receive";
                webhook.Expiration = DateTime.UtcNow.AddDays(30);

                // Content type for the subscription payload
                string contentType = "application/json";

                // Enable subscription service with specified update channels
                activityClient.StartSubscription(contentType, webhook);
                Console.WriteLine("Subscription service enabled successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}