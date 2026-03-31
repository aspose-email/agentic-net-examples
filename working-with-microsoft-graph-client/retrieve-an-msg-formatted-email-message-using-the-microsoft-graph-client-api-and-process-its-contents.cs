using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Clients;

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
            string userId = "user@example.com"; // UPN or user ID
            string messageId = "AAMkAG..."; // Message ID (ItemId) to fetch

            // Guard against placeholder credentials to avoid external calls during CI.
            if (clientId.StartsWith("your-") ||
                clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph call.");
                return;
            }

            // Prepare output path.
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDirectory, "message.msg");

            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create token provider.
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Create Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                try
                {
                    // Set the user context.
                    client.ResourceId = userId;

                    // Fetch the MSG-formatted message as a MapiMessage.
                    MapiMessage message = client.FetchMessage(messageId);

                    // Save the message to MSG file.
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
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
