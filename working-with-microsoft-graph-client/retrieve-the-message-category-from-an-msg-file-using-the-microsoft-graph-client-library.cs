using System;
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
            // Token provider for Microsoft Graph (dummy credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Create Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // ID of the message to fetch (replace with actual ID)
                string messageId = "message-id";

                // Fetch the message as a MapiMessage
                using (MapiMessage message = graphClient.FetchMessage(messageId))
                {
                    // Retrieve categories (may be null)
                    string[] categories = message.Categories;

                    if (categories != null && categories.Length > 0)
                    {
                        Console.WriteLine("Message Categories:");
                        foreach (string category in categories)
                        {
                            Console.WriteLine("- " + category);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No categories assigned to this message.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}