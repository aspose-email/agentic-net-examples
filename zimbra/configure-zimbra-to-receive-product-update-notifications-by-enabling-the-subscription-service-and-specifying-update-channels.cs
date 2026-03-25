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
            // Zimbra-like subscription configuration using Aspose.Email Activity client
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string serviceUrl = "https://outlook.office365.com/api/v2.0";

            // Create token provider for authentication
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize activity client
            using (IActivityClient activityClient = ActivityClient.GetClient(tokenProvider, serviceUrl))
            {
                // Configure webhook for receiving notifications
                Webhook webhook = new Webhook
                {
                    Address = "https://yourapp.example.com/webhook/receive",
                    Expiration = DateTime.UtcNow.AddDays(30)
                };

                // Enable subscription service for product update notifications
                string contentType = "productUpdates"; // channel identifier
                activityClient.StartSubscription(contentType, webhook);

                Console.WriteLine("Subscription to product updates enabled successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}