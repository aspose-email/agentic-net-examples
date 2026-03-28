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
            // Initialize token provider (dummy credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com/v1.0"))
            {
                // List messages from the Inbox folder (folder identifier can be "Inbox")
                MessageInfoCollection messages = client.ListMessages("Inbox");

                // Display subject of each message
                foreach (MessageInfo info in messages)
                {
                    // Use available properties of MessageInfo (e.g., Subject)
                    Console.WriteLine("Subject: " + info.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
