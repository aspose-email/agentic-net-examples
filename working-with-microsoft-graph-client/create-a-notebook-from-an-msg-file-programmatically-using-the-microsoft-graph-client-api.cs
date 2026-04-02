using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Guard against placeholder credentials to avoid live calls in CI.
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-") || tenantId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Initialize token provider.
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            // Create Graph client.
            IGraphClient client;
            try
            {
                client = GraphClient.GetClient(tokenProvider, tenantId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Path to the MSG file.
                string msgPath = "sample.msg";

                // Verify the MSG file exists.
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

                    Console.Error.WriteLine($"Input file not found: {msgPath}");
                    return;
                }

                // Load the MSG file into a MapiMessage.
                MapiMessage mapMessage;
                try
                {
                    mapMessage = MapiMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                    return;
                }

                using (mapMessage)
                {
                    // Create a Notebook object using the message subject as the notebook name.
                    Notebook newNotebook = new Notebook
                    {
                        DisplayName = mapMessage.Subject ?? "Untitled Notebook"
                    };

                    // Create the notebook via Graph client.
                    Notebook createdNotebook;
                    try
                    {
                        createdNotebook = client.CreateNotebook(newNotebook);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create notebook: {ex.Message}");
                        return;
                    }

                    // Output the created notebook details.
                    Console.WriteLine($"Notebook created successfully. ID: {createdNotebook.Id}, Name: {createdNotebook.DisplayName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
