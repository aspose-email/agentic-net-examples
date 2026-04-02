using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder authentication parameters
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";
            string userEmail = "user@example.com";

            // Skip execution if placeholders are not replaced
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") || tenantId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Authentication parameters are placeholders. Skipping Graph operations.");
                return;
            }

            // Path to the MSG file to be uploaded
            string msgPath = "SampleMessage.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
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
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "Placeholder body.";
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MailMessage instance
            MailMessage mailMessage;
            try
            {
                MapiMessage mapiMsg = MapiMessage.Load(msgPath);
                mailMessage = mapiMsg.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Create token provider for Outlook (MSAL)
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Initialize Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Set the user (resource) for which the operations will be performed
                client.ResourceId = userEmail;

                // Target folder ID (e.g., Inbox). In a real scenario, retrieve the actual folder ID.
                string folderId = "Inbox";

                try
                {
                    // Create the message in the specified folder
                    client.CreateMessage(folderId, mailMessage);
                    Console.WriteLine("Message created successfully in folder: " + folderId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create message via Graph: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
