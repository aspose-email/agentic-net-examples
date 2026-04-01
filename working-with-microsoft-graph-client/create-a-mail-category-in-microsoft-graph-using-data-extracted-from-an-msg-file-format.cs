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
            // Input MSG file path
            string msgPath = "sample.msg";

            // Guard file existence
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

                // Create a minimal placeholder MSG file
                using (var placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "No content"))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the MSG file
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

            // Extract the first category from the message (if any)
            string categoryName = "DefaultCategory";
            try
            {
                var categories = FollowUpManager.GetCategories(mapiMessage);
                if (categories != null && categories.Count > 0)
                {
                    categoryName = categories[0];
                }
            }
            catch
            {
                // Ignore extraction errors and use default name
            }

            // Placeholder credentials – skip real network call if not replaced
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Create token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
            {
                try
                {
                    // Create a new Outlook category in the user's master list
                    OutlookCategory createdCategory = client.CreateCategory(categoryName, CategoryPreset.Preset0);
                    Console.WriteLine($"Category created: {createdCategory.DisplayName}, Id: {createdCategory.Id}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                }
            }

            // Dispose the loaded message
            mapiMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
