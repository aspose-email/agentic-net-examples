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
            // Paths for input MSG and output EML files
            string msgPath = "sample.msg";
            string emlPath = "sample.eml";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Initialize the Microsoft Graph client
                IGraphClient client;
                try
                {
                    // Create a token provider with dummy credentials
                    Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                        "clientId", "clientSecret", "refreshToken");
                    // Dummy tenant identifier
                    string tenantId = "tenantId";

                    client = GraphClient.GetClient(tokenProvider, tenantId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error initializing Graph client: {ex.Message}");
                    return;
                }

                // Example usage of the Graph client (sending the message as MIME)
                try
                {
                    client.SendAsMime(mapiMessage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message via Graph: {ex.Message}");
                    // Continue with conversion even if sending fails
                }

                // Convert the MapiMessage to a MailMessage
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    // Save the MailMessage as an EML file
                    try
                    {
                        mailMessage.Save(emlPath, SaveOptions.DefaultEml);
                        Console.WriteLine($"Converted EML saved to {emlPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving EML file: {ex.Message}");
                    }
                }

                // Dispose the Graph client
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
