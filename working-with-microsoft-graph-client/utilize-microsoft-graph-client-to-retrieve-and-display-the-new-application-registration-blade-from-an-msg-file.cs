using System;
using System.IO;
using System.Net;
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
            // File path to the MSG file
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Create Outlook token provider (replace with real credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Tenant identifier (replace with real tenant ID)
            string tenantId = "tenantId";

            // Initialize Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Load the MSG file as a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                {
                    // Display basic properties of the message
                    Console.WriteLine("Subject: " + mapiMessage.Subject);
                    Console.WriteLine("From: " + mapiMessage.SenderEmailAddress);
                    Console.WriteLine("To: " + string.Join("; ", mapiMessage.Recipients));
                    Console.WriteLine("Body:");
                    Console.WriteLine(mapiMessage.Body);

                    // Example: upload the message to the user's Drafts folder via Graph
                    // (Folder ID for Drafts can be obtained via graphClient.ListFolders or known constant)
                    // Here we use the simple overload that creates a message from MapiMessage
                    string draftsFolderId = "drafts";
                    graphClient.CreateMessage(draftsFolderId, mapiMessage);
                    Console.WriteLine("Message uploaded to Drafts folder.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}