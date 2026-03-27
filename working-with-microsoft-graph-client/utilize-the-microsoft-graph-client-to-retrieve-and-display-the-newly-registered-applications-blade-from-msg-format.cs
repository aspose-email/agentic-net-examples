using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

public class Program
{
    public static void Main()
    {
        try
        {
            // Placeholder credentials for token provider
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Create token provider (Outlook provider expects exactly three arguments)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client using the token provider and tenant identifier
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Retrieve messages from the Inbox folder (folder identifier can be "Inbox")
                MessageInfoCollection messages = graphClient.ListMessages("Inbox");

                // Display basic information for each message
                foreach (MessageInfo message in messages)
                {
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("ItemId: " + message.ItemId);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}