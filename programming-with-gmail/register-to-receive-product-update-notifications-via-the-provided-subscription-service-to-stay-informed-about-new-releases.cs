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
            // Create token provider for Outlook (replace with real credentials)
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                clientId: "clientId",
                clientSecret: "clientSecret",
                refreshToken: "refreshToken");

            // Service URL for the subscription service (placeholder)
            string serviceUrl = "https://api.example.com/activity";

            // Create the Activity client
            using (IActivityClient client = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Prepare webhook information
                var webhook = new Webhook
                {
                    Address = "https://yourapp.example.com/webhook",
                    Expiration = DateTime.UtcNow.AddHours(1)
                };

                // Register for product update notifications (content type is a placeholder)
                client.StartSubscription("product-updates", webhook);

                Console.WriteLine("Subscription request sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
