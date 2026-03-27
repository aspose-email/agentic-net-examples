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
            // Initialize token provider (Outlook) with dummy credentials
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Create Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // Example usage of the client (display endpoint)
                Console.WriteLine("Graph endpoint: " + graphClient.EndPoint);
            }

            // Path to the MSG file
            string msgPath = "sample.msg";

            // Guard file existence
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load MSG file and convert to MailMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    Console.WriteLine("Subject: " + mailMessage.Subject);
                    Console.WriteLine("Body: " + mailMessage.Body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
