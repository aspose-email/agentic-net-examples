using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Skip execution if placeholder credentials are detected.
            if (clientId.Contains("your-") || clientSecret.Contains("your-") || refreshToken.Contains("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Create the Outlook token provider.
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, tenantId);

            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                try
                {
                    // Define the sender address and desired classification.
                    MailAddress sender = new MailAddress("sender@example.com");
                    ClassificationType classification = ClassificationType.Focused; // Example classification.

                    // Create or update the classification override for the sender.
                    client.CreateOrUpdateOverride(sender, classification);
                    Console.WriteLine("Classification override created or updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
