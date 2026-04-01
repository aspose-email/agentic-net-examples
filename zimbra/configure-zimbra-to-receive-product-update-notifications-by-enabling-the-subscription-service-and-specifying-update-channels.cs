using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Activity;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            const string clientId = "YOUR_CLIENT_ID";
            const string clientSecret = "YOUR_CLIENT_SECRET";
            const string refreshToken = "YOUR_REFRESH_TOKEN";
            const string serviceUrl = "https://zimbra.example.com/api";

            // Guard against placeholder credentials to avoid real network calls during CI.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Zimbra subscription configuration.");
                return;
            }

            // Obtain a token provider for Outlook (used here as an example; adjust for Zimbra if needed).
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Create the activity client. The factory returns an IActivityClient implementation.
            using (IActivityClient client = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                try
                {
                    // Define the webhook that will receive product update notifications.
                    var webhook = new Webhook
                    {
                        Address = "https://yourapp.example.com/webhook",
                        Expiration = DateTime.UtcNow.AddDays(7)
                    };

                    // Enable the subscription service for the "productUpdates" content type.
                    client.StartSubscription("productUpdates", webhook);

                    Console.WriteLine("Subscription to product update notifications has been enabled.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while configuring subscription: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
