using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file
                string msgFilePath = "sample.msg";

                // Azure AD application credentials (replace with real values)
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";

                // Verify that the MSG file exists
                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgFilePath}");
                    return;
                }

                // Create the Outlook token provider
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

                // Initialize the Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Load the MSG file into a MailMessage object
                    using (MailMessage mailMessage = MailMessage.Load(msgFilePath))
                    {
                        // Upload the message to the Inbox folder
                        try
                        {
                            graphClient.CreateMessage("Inbox", mailMessage);
                            Console.WriteLine("Message uploaded successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to upload message: {ex.Message}");
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
}