using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;

class Program
{
    static void Main()
    {
        try
        {
            // Create token provider for Outlook (replace placeholders with real values)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "YOUR_CLIENT_ID",
                "YOUR_CLIENT_SECRET",
                "YOUR_REFRESH_TOKEN");

            // Initialize Activity client (replace service URL if needed)
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, "https://outlook.office365.com"))
            {
                // Configure webhook for receiving notifications
                Webhook webhook = new Webhook();
                webhook.Address = "https://yourapp.example.com/webhook/receive";
                webhook.Expiration = DateTime.UtcNow.AddDays(30);

                // Register subscription (content type can be adjusted as required)
                activityClient.StartSubscription("application/json", webhook);

                Console.WriteLine("Subscription created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}