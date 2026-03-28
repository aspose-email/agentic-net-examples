using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Create Outlook token provider (3‑argument overload)
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Initialize Graph client for a specific tenant
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // List messages from the Inbox folder (folder identifier is the well‑known name)
                MessageInfoCollection messages = client.ListMessages("Inbox");

                foreach (MessageInfo message in messages)
                {
                    // Output basic information; avoid using non‑existent Id property
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
