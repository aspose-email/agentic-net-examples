using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "Placeholder Subject",
                    "Placeholder Body",
                    "sender@example.com",
                    "receiver@example.com"))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Prepare token provider for Microsoft Graph (dummy credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider =
                    Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                        "clientId",
                        "clientSecret",
                        "refreshToken");

                // Initialize Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Target folder in the mailbox (e.g., Inbox)
                    string folderId = "Inbox";

                    // Create the message in the specified folder
                    MapiMessage createdMessage = client.CreateMessage(folderId, msg);
                    Console.WriteLine($"Message created. ItemId: {createdMessage.ItemId}");

                    // Fetch the created message back from Graph
                    MapiMessage fetchedMessage = client.FetchMessage(createdMessage.ItemId);
                    Console.WriteLine($"Fetched message subject: {fetchedMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
