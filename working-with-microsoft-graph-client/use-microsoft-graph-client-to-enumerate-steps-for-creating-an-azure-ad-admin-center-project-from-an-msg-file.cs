using Aspose.Email.Mapi;
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
            // Placeholder credentials for Azure AD app registration
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Detect placeholder credentials and skip live calls
            if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Create token provider (wrapped in try/catch for safety)
            TokenProvider tokenProvider;
            try
            {
                tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            // Initialize Graph client (wrapped in try/catch for safety)
            IGraphClient client;
            try
            {
                client = GraphClient.GetClient(tokenProvider, tenantId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to initialize Graph client: {ex.Message}");
                return;
            }

            // Ensure proper disposal of the client
            using (client)
            {
                // Path to the MSG file
                string msgPath = "sample.msg";

                // Guard file existence; create minimal placeholder if missing
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    try
                    {
                        using (MailMessage placeholder = new MailMessage("placeholder@example.com", "placeholder@example.com", "Placeholder", "This is a placeholder MSG."))
                        {
                            placeholder.Save(msgPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                        return;
                    }
                }

                // Load the MSG file into a MapiMessage (wrapped in try/catch)
                MapiMessage mapiMessage;
                try
                {
                    mapiMessage = MapiMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                    return;
                }

                // Ensure proper disposal of the MapiMessage
                using (mapiMessage)
                {
                    // Upload the message to the user's Inbox folder via Graph
                    try
                    {
                        client.CreateMessage("Inbox", mapiMessage);
                        Console.WriteLine("Message uploaded to Microsoft Graph successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create message in Graph: {ex.Message}");
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
