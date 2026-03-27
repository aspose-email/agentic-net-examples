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
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                MailAddress senderAddress = mailMessage.From;
                Console.WriteLine($"Sender: {senderAddress.Address}");

                // Prepare token provider (replace placeholder values with real credentials)
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Tenant identifier (replace with actual tenant ID)
                string tenantId = "tenantId";

                // Initialize Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Create or update an override for the sender.
                    // NOTE: Replace the cast with a valid ClassificationType enum value as required.
                    ClassificationType classification = (ClassificationType)0;
                    graphClient.CreateOrUpdateOverride(new MailAddress(senderAddress.Address), classification);
                    Console.WriteLine("Override created or updated successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}