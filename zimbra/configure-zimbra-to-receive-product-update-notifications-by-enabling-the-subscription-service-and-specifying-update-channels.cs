using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;
using Aspose.Email.Clients.Base;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider for Outlook (replace with actual credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "your-client-id",
                "your-client-secret",
                "your-refresh-token");

            // Create Activity client for Zimbra (replace with actual service URL)
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, "https://activity.zimbra.com"))
            {
                try
                {
                    // Configure webhook for receiving notifications
                    Webhook webhook = new Webhook();
                    webhook.Address = "https://myapp.example.com/webhook";
                    webhook.Expiration = DateTime.UtcNow.AddDays(30);

                    // Enable subscription service for product updates
                    string contentType = "product-updates"; // channel identifier
                    activityClient.StartSubscription(contentType, webhook);

                    Console.WriteLine("Subscription to product update notifications enabled successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while configuring subscription: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
            return;
        }
    }
}