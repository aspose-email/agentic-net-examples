using Aspose.Email.Mapi;
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

            // Skip execution when placeholders are detected.
            if (clientId.Contains("your-") || clientSecret.Contains("your-") ||
                refreshToken.Contains("your-") || tenantId.Contains("your-"))
            {
                Console.WriteLine("Placeholder credentials detected – aborting example.");
                return;
            }

            // Create token provider for Outlook (Microsoft Graph) authentication.
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Path to the MSG file to be uploaded.
                string msgPath = "sample.msg";

                // Verify the MSG file exists.
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Error: File not found – {msgPath}");
                    return;
                }

                // Load the MSG file into a MapiMessage.
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    // Upload the message to the Inbox folder (use folder ID or well‑known name).
                    // Here we use the well‑known folder name "Inbox".
                    client.CreateMessage("Inbox", msg);
                    Console.WriteLine("Message uploaded successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
