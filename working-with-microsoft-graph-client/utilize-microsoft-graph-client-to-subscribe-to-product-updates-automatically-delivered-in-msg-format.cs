using System;
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
            // Initialize token provider for Outlook (placeholder credentials)
            using (TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance("clientId", "clientSecret", "refreshToken"))
            {
                // Create Graph client with the token provider and tenant identifier
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Create a MAPI message (MSG format) representing the product update
                    MapiMessage productUpdate = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Product Update Notification",
                        "Please find the latest product update attached."
                    );

                    // Send the message using the Graph client (delivered as MSG)
                    graphClient.SendAsMime(productUpdate);
                    Console.WriteLine("Product update sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}