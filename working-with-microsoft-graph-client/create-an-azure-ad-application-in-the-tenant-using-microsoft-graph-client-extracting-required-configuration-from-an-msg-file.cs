using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgPath = "config.msg";

                // Verify the MSG file exists
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Load the MSG file as a MailMessage
                using (MailMessage mailMessage = MailMessage.Load(msgPath))
                {
                    // Extract configuration values from the message body
                    string clientId = null;
                    string clientSecret = null;
                    string refreshToken = null;
                    string tenantId = null;

                    string[] lines = mailMessage.Body.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("ClientId=", StringComparison.OrdinalIgnoreCase))
                            clientId = line.Substring("ClientId=".Length).Trim();
                        else if (line.StartsWith("ClientSecret=", StringComparison.OrdinalIgnoreCase))
                            clientSecret = line.Substring("ClientSecret=".Length).Trim();
                        else if (line.StartsWith("RefreshToken=", StringComparison.OrdinalIgnoreCase))
                            refreshToken = line.Substring("RefreshToken=".Length).Trim();
                        else if (line.StartsWith("TenantId=", StringComparison.OrdinalIgnoreCase))
                            tenantId = line.Substring("TenantId=".Length).Trim();
                    }

                    // Validate extracted configuration
                    if (string.IsNullOrEmpty(clientId) ||
                        string.IsNullOrEmpty(clientSecret) ||
                        string.IsNullOrEmpty(refreshToken) ||
                        string.IsNullOrEmpty(tenantId))
                    {
                        Console.Error.WriteLine("Required configuration not found in the message.");
                        return;
                    }

                    // Create Outlook token provider (IDisposable)
                    using (TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken))
                    {
                        // Initialize Graph client (IDisposable)
                        using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                        {
                            // Placeholder for Azure AD application creation using Graph API
                            Console.WriteLine("Graph client initialized. Ready to create Azure AD application.");
                            // Example (unverified): graphClient.CreateFolder("MyAppFolder");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}