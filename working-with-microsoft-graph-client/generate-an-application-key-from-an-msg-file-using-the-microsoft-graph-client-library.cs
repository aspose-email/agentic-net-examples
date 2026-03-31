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
            // Input MSG file path
            string msgFilePath = "message.msg";

            // Guard file existence
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load MSG into MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
            {
                // Placeholder credentials – replace with real values
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";

                // Skip execution if placeholders are still present
                if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Please provide valid Outlook OAuth credentials before running this sample.");
                    return;
                }

                // Create token provider
                Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Create Graph client (default endpoint)
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
                {
                    // Target folder – using Drafts as an example
                    string targetFolderId = "Drafts";

                    // Upload the message to the folder
                    MapiMessage createdMessage = client.CreateMessage(targetFolderId, mapiMessage);

                    // The ItemId serves as the application key
                    string applicationKey = createdMessage.ItemId;

                    Console.WriteLine($"Application key (ItemId) generated: {applicationKey}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
