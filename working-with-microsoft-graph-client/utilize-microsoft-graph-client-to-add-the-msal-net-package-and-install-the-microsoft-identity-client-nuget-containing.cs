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
            // Initialize Outlook token provider (clientId, clientSecret, refreshToken)
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Create Graph client with token provider and tenant ID
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // List messages from the Inbox folder
                foreach (MessageInfo messageInfo in client.ListMessages("Inbox"))
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
