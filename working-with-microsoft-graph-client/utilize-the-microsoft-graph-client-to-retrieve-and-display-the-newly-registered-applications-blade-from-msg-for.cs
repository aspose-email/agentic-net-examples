using Aspose.Email.Mapi;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values when available.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";
            string messageId = "your-message-id";

            // Skip actual network call if placeholders are detected.
            if (clientId.Contains("your-") || clientSecret.Contains("your-") ||
                refreshToken.Contains("your-") || tenantId.Contains("your-") ||
                messageId.Contains("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operation.");
                return;
            }

            // Create Outlook token provider.
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Fetch the message by its ID.
                MapiMessage message = client.FetchMessage(messageId);

                // Display basic information about the message.
                Console.WriteLine("Subject: " + message.Subject);
                Console.WriteLine("From: " + message.SenderEmailAddress);
                string bodyPreview = message.Body != null
                    ? (message.Body.Length > 100 ? message.Body.Substring(0, 100) + "..." : message.Body)
                    : string.Empty;
                Console.WriteLine("Body Preview: " + bodyPreview);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
