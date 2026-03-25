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
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Create token provider for Outlook
            using (TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken))
            {
                // Create activity client
                using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
                {
                    // Configure webhook for subscription
                    Aspose.Email.Clients.Activity.Webhook webhook = new Aspose.Email.Clients.Activity.Webhook();
                    webhook.Address = "https://yourapp.example.com/webhook";
                    webhook.Expiration = DateTime.UtcNow.AddDays(30);

                    // Start subscription for product update notifications
                    activityClient.StartSubscription("ProductUpdates", webhook);
                    Console.WriteLine("Product update subscription started successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}