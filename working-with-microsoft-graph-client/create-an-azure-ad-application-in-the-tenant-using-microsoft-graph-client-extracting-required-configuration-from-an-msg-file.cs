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
            // Path to the MSG file containing configuration
            string msgFilePath = "config.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Configuration file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file and extract required settings
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string refreshToken = string.Empty;
            string tenantId = string.Empty;

            try
            {
                using (MailMessage configMessage = MailMessage.Load(msgFilePath))
                {
                    // Expect each line in the body to be in the format Key=Value
                    string[] lines = configMessage.Body.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length != 2) continue;
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        switch (key)
                        {
                            case "ClientId":
                                clientId = value;
                                break;
                            case "ClientSecret":
                                clientSecret = value;
                                break;
                            case "RefreshToken":
                                refreshToken = value;
                                break;
                            case "TenantId":
                                tenantId = value;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or parse MSG file: {ex.Message}");
                return;
            }

            // Validate extracted configuration
            if (string.IsNullOrEmpty(clientId) ||
                string.IsNullOrEmpty(clientSecret) ||
                string.IsNullOrEmpty(refreshToken) ||
                string.IsNullOrEmpty(tenantId))
            {
                Console.Error.WriteLine("Missing required configuration values in the MSG file.");
                return;
            }

            // Create Outlook token provider
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId,
                clientSecret,
                refreshToken);

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

            // Use the client to create an Azure AD application
            try
            {
                using (client)
                {
                    // Prepare the JSON payload for the new application
                    string appName = "MyAsposeApp";
                    string jsonPayload = $"{{\"displayName\":\"{appName}\",\"signInAudience\":\"AzureADMyOrg\"}}";

                    // NOTE: The IGraphClient interface does not expose a direct method for creating Azure AD applications.
                    // You would typically use a generic request method or an HTTP client to POST to "/applications".
                    // The following is a placeholder for the actual implementation:
                    // client.Post("/applications", jsonPayload);

                    Console.WriteLine("Graph client initialized. Ready to create Azure AD application.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while creating Azure AD application: {ex.Message}");
                // client will be disposed by the using block
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
