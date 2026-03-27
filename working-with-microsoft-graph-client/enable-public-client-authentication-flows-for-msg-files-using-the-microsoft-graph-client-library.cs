using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

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
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Create a token provider for public client authentication (dummy credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken"
            );

            // Initialize the Graph client using the token provider
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                try
                {
                    // Load the MSG file into a MapiMessage
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        // Send the message via Microsoft Graph using MIME format
                        client.SendAsMime(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during message processing: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
