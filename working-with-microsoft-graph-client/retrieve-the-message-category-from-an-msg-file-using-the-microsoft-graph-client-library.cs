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
            // Placeholder credentials – skip actual network call if not replaced
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            if (clientId == "your-client-id" ||
                clientSecret == "your-client-secret" ||
                refreshToken == "your-refresh-token")
            {
                Console.Error.WriteLine("Placeholder Graph credentials detected. Skipping Graph operations.");
                return;
            }

            // Path to the MSG file
            string msgPath = "message.msg";

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

                Console.Error.WriteLine($"MSG file not found at path: {msgPath}");
                return;
            }

            // Load the MSG file
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Retrieve the first category name from the message (if any)
            string[] categories = msg.Categories;
            if (categories == null || categories.Length == 0)
            {
                Console.WriteLine("No categories assigned to the message.");
                return;
            }

            string categoryName = categories[0];
            Console.WriteLine($"Category name from MSG: {categoryName}");

            // Create token provider and Graph client
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                try
                {
                    // Fetch the category details from Graph using the category name as ID placeholder
                    // In a real scenario, replace 'categoryId' with the actual OutlookCategory ID.
                    string categoryId = categoryName; // placeholder mapping
                    OutlookCategory fetchedCategory = client.FetchCategory(categoryId);
                    Console.WriteLine($"Fetched Category Display Name: {fetchedCategory.DisplayName}");
                    Console.WriteLine($"Fetched Category Color: {fetchedCategory.Color}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
