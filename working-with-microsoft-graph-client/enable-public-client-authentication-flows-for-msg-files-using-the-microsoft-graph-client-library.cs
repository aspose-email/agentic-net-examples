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
            string msgPath = "sample.msg";
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load MSG file into a MailMessage object
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Prepare token provider for public client authentication flow
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId", "clientSecret", "refreshToken");

            // Tenant identifier (e.g., tenant GUID or domain)
            string tenantId = "your-tenant-id";

            // Initialize Graph client
            IGraphClient client;
            try
            {
                client = GraphClient.GetClient(tokenProvider, tenantId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating Graph client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Upload the message to the Drafts folder
                try
                {
                    client.CreateMessage("Drafts", mailMessage);
                    Console.WriteLine("Message uploaded to Drafts successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
