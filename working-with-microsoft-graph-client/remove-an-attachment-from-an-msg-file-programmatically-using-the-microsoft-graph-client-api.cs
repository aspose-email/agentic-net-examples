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
            // Verify that a placeholder MSG file exists (required for the example flow)
            string msgFilePath = "sample.msg";
            if (!File.Exists(msgFilePath))
            {
                // Create a minimal MSG file to avoid missing file errors
                MapiMessage placeholder = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder", "Body");
                placeholder.Save(msgFilePath);
                Console.WriteLine("Created placeholder MSG file: " + msgFilePath);
            }

            // Graph authentication parameters (dummy values for illustration)
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // The ID of the message stored in Microsoft Graph (replace with a real ID)
            string messageId = "message-id";

            // The name of the attachment to remove
            string attachmentFileNameToRemove = "attachment-to-delete.txt";

            // Create token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // List attachments of the specified message
                MapiAttachmentCollection attachments = graphClient.ListAttachments(messageId);

                // Find the attachment with the specified file name
                foreach (MapiAttachment attachment in attachments)
                {
                    Console.WriteLine("Found attachment: " + attachment.FileName);
                    if (string.Equals(attachment.FileName, attachmentFileNameToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        // Delete the attachment using its Graph item ID
                        graphClient.DeleteAttachment(attachment.ItemId);
                        Console.WriteLine("Deleted attachment: " + attachment.FileName);
                        break;
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