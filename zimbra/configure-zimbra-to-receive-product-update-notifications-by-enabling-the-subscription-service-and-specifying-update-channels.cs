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
            // Prepare OAuth token provider for Outlook (used by Activity service)
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "your-client-id",
                "your-client-secret",
                "your-refresh-token"
            );

            // Initialize Activity client (disposable)
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, "https://activity.zimbra.com"))
            {
                // Configure webhook that will receive notifications
                Webhook webhook = new Webhook
                {
                    Address = "https://myapp.example.com/webhook",
                    Expiration = DateTime.UtcNow.AddDays(30)
                };

                // Enable subscription for product update notifications
                activityClient.StartSubscription("ProductUpdates", webhook);
                Console.WriteLine("Zimbra product update subscription enabled.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}