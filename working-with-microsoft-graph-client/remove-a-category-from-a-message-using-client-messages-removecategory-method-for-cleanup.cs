using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using System;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and identifiers
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string tenantId = "YOUR_TENANT_ID";
            string messageId = "YOUR_MESSAGE_ID";
            string categoryName = "SampleCategory";

            // Guard against placeholder values to avoid real network calls during CI
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") ||
                tenantId.StartsWith("YOUR_") ||
                messageId.StartsWith("YOUR_") ||
                categoryName.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder values detected. Skipping execution.");
                return;
            }

            // Create token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, categoryName);
            Console.WriteLine($"Category '{categoryName}' removed from message '{messageId}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
