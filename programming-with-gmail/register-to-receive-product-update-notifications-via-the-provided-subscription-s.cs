using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;

class Program
{
    static void Main()
    {
        try
        {
            // Obtain an Outlook token provider (replace placeholders with real values)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // URL of the subscription service (replace with actual service endpoint)
            string serviceUrl = "https://activity.example.com";

            // Create the activity client and ensure it is disposed properly
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure the webhook that will receive notifications
                Webhook webhook = new Webhook
                {
                    Address = "https://myapp.example.com/webhook",
                    Expiration = DateTime.UtcNow.AddDays(30)
                };

                // Define the content type for product update notifications
                string contentType = "productUpdates";

                // Register the subscription
                activityClient.StartSubscription(contentType, webhook);
                Console.WriteLine("Subscription registered successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
