using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace RetrieveMsgCategory
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file
                string msgPath = "message.msg";

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

                    Console.Error.WriteLine($"Input file not found: {msgPath}");
                    return;
                }

                // Load the MSG file
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Retrieve categories from the message using FollowUpManager
                IList<string> categories = FollowUpManager.GetCategories(msg);

                Console.WriteLine("Categories found in the MSG file:");
                foreach (string category in categories)
                {
                    Console.WriteLine($"- {category}");
                }

                // -------------------------------------------------
                // Example of creating a Microsoft Graph client
                // (credentials are placeholders and should be replaced with real values)
                // -------------------------------------------------
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string tenantId = "your-tenant-id";
                string refreshToken = "your-refresh-token";

                // Obtain a token provider for Outlook (Microsoft Graph)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Create the Graph client (implements IDisposable)
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com/v1.0"))
                {
                    // Example: fetch a specific Outlook category by its ID
                    // Replace with a valid category ID if needed
                    // string categoryId = "your-category-id";
                    // OutlookCategory outlookCategory = graphClient.FetchCategory(categoryId);
                    // Console.WriteLine($"Fetched category: {outlookCategory.DisplayName}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
