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
            // Path to the MSG file
            string msgPath = "message.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Create a token provider for Outlook (3‑argument overload)
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Initialize the Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Send the message via Graph as MIME
                    client.SendAsMime(mapiMessage);
                    Console.WriteLine("Message sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
