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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage object
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Create a token provider for Outlook (3‑argument overload)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    clientId: "clientId",
                    clientSecret: "clientSecret",
                    refreshToken: "refreshToken");

                // Tenant identifier (replace with actual tenant ID)
                string tenantId = "yourTenantId";

                // Initialize the Microsoft Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Upload the message to the Drafts folder
                    client.CreateMessage("Drafts", message);
                    Console.WriteLine("Message uploaded to Drafts folder successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
