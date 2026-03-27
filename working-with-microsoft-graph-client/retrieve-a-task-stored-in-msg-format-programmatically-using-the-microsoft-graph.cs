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
            // Replace the placeholders with actual values.
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";
            string messageId = "messageId"; // The Graph message ID that contains the task in MSG format.

            // Create a token provider for Microsoft Graph.
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Initialize the Graph client.
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Fetch the MSG message as a MapiMessage.
                MapiMessage msg = graphClient.FetchMessage(messageId);

                // Verify that the message represents a task.
                if (msg.SupportedType == MapiItemType.Task)
                {
                    // Convert the MapiMessage to a MapiTask.
                    MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                    // Output some task properties.
                    Console.WriteLine("Task Subject: " + task.Subject);
                    Console.WriteLine("Due Date: " + task.DueDate);
                    Console.WriteLine("Percent Complete: " + task.PercentComplete);
                }
                else
                {
                    Console.WriteLine("The fetched message is not a task.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}