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
            // Define paths and credentials (dummy values for illustration)
            string messageFilePath = "sample.msg";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(messageFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test body"))
                    {
                        placeholder.Save(messageFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder MSG file: " + ex.Message);
                    return;
                }
            }

            // Load the MAPI message from the file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(messageFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to load MSG file: " + ex.Message);
                return;
            }

            // Create the token provider (Outlook provider expects three arguments)
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create token provider: " + ex.Message);
                return;
            }

            // Initialize the Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                try
                {
                    // Send the message using the Graph client
                    graphClient.Send(message);
                    Console.WriteLine("Message sent successfully via Microsoft Graph.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error sending message: " + ex.Message);
                }
            }

            // Dispose the loaded message
            if (message != null)
            {
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}