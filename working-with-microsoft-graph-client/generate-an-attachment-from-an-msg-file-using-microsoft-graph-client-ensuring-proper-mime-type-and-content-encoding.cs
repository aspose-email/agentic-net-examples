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
            const string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Create a token provider (dummy credentials)
                using (Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId", "clientSecret", "refreshToken"))
                {
                    // Initialize the Graph client
                    using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
                    {
                        try
                        {
                            // Send the message using MIME format
                            client.SendAsMime(mapiMessage);
                            Console.WriteLine("Message sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error sending message: {ex.Message}");
                        }
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
