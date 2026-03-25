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
            // OAuth token provider for Outlook (used by Activity service)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId: "your-client-id",
                clientSecret: "your-client-secret",
                refreshToken: "your-refresh-token"
            );

            // Activity service endpoint (Zimbra activity service URL)
            string serviceUrl = "https://zimbra.example.com/activity";

            // Create Activity client
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure webhook for receiving notifications
                Webhook webhook = new Webhook();
                webhook.Address = "https://yourapp.example.com/webhook/receive";
                webhook.Expiration = DateTime.UtcNow.AddDays(30);

                // Enable subscription for product update notifications
                // Content type can be any identifier understood by Zimbra; using "product-updates" as example
                activityClient.StartSubscription("product-updates", webhook);

                Console.WriteLine("Subscription for product updates has been enabled.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}