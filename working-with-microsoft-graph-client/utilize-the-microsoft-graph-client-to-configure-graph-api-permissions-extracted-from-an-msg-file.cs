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
            // Paths for the MSG file
            const string msgPath = "sample.msg";

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

                try
                {
                    using (var placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder MSG file.";
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
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
                // Extract categories from the message
                string[] categories = msg.Categories ?? Array.Empty<string>();
                if (categories.Length == 0)
                {
                    Console.WriteLine("No categories found in the MSG file.");
                    return;
                }

                // Placeholder token provider credentials
                const string clientId = "your-client-id";
                const string clientSecret = "your-client-secret";
                const string refreshToken = "your-refresh-token";
                const string tenantId = "your-tenant-id";

                // Guard against placeholder credentials
                if (clientId.StartsWith("your-") ||
                    clientSecret.StartsWith("your-") ||
                    refreshToken.StartsWith("your-") ||
                    tenantId.StartsWith("your-"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Graph API calls.");
                    return;
                }

                // Create token provider
                Aspose.Email.Clients.ITokenProvider tokenProvider;
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
                    // Create each category in the user's master list
                    foreach (string category in categories)
                    {
                        try
                        {
                            client.CreateCategory(category, CategoryPreset.None);
                            Console.WriteLine($"Created category: {category}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create category '{category}': {ex.Message}");
                        }
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
