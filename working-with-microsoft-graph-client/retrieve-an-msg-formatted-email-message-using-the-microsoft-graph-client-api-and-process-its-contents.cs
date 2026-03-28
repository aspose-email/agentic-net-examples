using System;
using System.IO;
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
            // Create token provider for Outlook (Microsoft Graph)
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",          // Replace with your Azure AD app client ID
                "clientSecret",      // Replace with your Azure AD app client secret
                "refreshToken");     // Replace with a valid refresh token

            string tenantId = "yourTenantId"; // Replace with your tenant ID (e.g., your domain)

            // Initialize Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // List messages in the Inbox folder
                var messages = client.ListMessages("Inbox");
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Take the first message
                MessageInfo messageInfo = messages[0];
                string messageId = messageInfo.ItemId;

                // Fetch the message as a MAPI message (MSG format)
                using (MapiMessage mapiMessage = client.FetchMessage(messageId))
                {
                    // Process the message contents
                    Console.WriteLine("Subject: " + mapiMessage.Subject);
                    Console.WriteLine("From: " + mapiMessage.SenderEmailAddress);
                    Console.WriteLine("Body Preview: " + (mapiMessage.Body?.Substring(0, Math.Min(100, mapiMessage.Body.Length)) ?? string.Empty));

                    // Define output file path
                    string outputPath = "output.msg";

                    // Ensure the directory exists
                    string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the message in MSG format
                    mapiMessage.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
