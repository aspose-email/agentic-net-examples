using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file containing Office credentials
            string msgPath = "sample.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Extract credentials from custom headers (replace with actual header names as needed)
            string clientId = mailMessage.Headers["X-ClientId"] ?? "clientId";
            string clientSecret = mailMessage.Headers["X-ClientSecret"] ?? "clientSecret";
            string refreshToken = mailMessage.Headers["X-RefreshToken"] ?? "refreshToken";
            string tenantId = mailMessage.Headers["X-TenantId"] ?? "tenantId";

            // Create a token provider for Outlook (Azure AD)
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize the Microsoft Graph client
            IGraphClient graphClient;
            try
            {
                graphClient = GraphClient.GetClient(tokenProvider, tenantId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                mailMessage.Dispose();
                return;
            }

            // Use the Graph client to list messages in the Inbox folder
            using (graphClient)
            {
                IEnumerable<MessageInfo> inboxMessages = graphClient.ListMessages("Inbox");
                foreach (MessageInfo info in inboxMessages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }

            // Dispose the loaded message
            mailMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
