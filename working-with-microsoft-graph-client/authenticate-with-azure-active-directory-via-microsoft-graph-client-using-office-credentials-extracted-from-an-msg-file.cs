using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file containing Office credentials
            string msgPath = "officeCredentials.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
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

                using (MapiMessage placeholder = new MapiMessage())
                {
                    placeholder.Subject = "Placeholder";
                    placeholder.Save(msgPath);
                }
                Console.Error.WriteLine($"Input MSG file not found. Created placeholder at '{msgPath}'.");
                return;
            }

            // Load the MSG file
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                // Extract credentials from custom properties (placeholder values used here)
                // In a real scenario, retrieve actual values from msg.Properties or custom properties.
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";

                // Guard against placeholder credentials to avoid live network calls
                if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-") || tenantId.StartsWith("your-"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph authentication.");
                    return;
                }

                // Create token provider for Outlook
                TokenProvider tokenProvider;
                try
                {
                    tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                    return;
                }

                // Initialize Graph client
                IGraphClient client;
                try
                {
                    client = GraphClient.GetClient(tokenProvider, tenantId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                    return;
                }

                using (client)
                {
                    // Example: fetch a message by its ID (placeholder ID used)
                    string messageId = "your-message-id";

                    if (messageId.StartsWith("your-"))
                    {
                        Console.Error.WriteLine("Placeholder message ID detected. Skipping fetch operation.");
                        return;
                    }

                    MapiMessage fetchedMessage;
                    try
                    {
                        fetchedMessage = client.FetchMessage(messageId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message: {ex.Message}");
                        return;
                    }

                    using (fetchedMessage)
                    {
                        Console.WriteLine($"Subject: {fetchedMessage.Subject}");
                        Console.WriteLine($"From: {fetchedMessage.SenderEmailAddress}");
                        Console.WriteLine($"Body: {fetchedMessage.Body}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
