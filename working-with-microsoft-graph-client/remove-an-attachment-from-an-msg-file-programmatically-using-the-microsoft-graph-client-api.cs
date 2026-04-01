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
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against executing with placeholder credentials.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please provide valid Microsoft Graph credentials before running the sample.");
                return;
            }

            // Create a token provider (3‑argument overload).
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize the Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                // Placeholder message identifier – replace with the actual message Id.
                string messageId = "MESSAGE_ID";

                // Fetch the message (optional, demonstrates retrieval).
                MapiMessage message = client.FetchMessage(messageId);

                // List attachments of the message.
                MapiAttachmentCollection attachments = client.ListAttachments(messageId);
                Console.WriteLine($"Found {attachments.Count} attachment(s) in the message.");

                foreach (MapiAttachment att in attachments)
                {
                    Console.WriteLine($"Attachment: {att.FileName}");
                }

                // Delete the first attachment as an example.
                if (attachments.Count > 0)
                {
                    // Replace with the actual attachment Id obtained from Graph.
                    string attachmentId = "ATTACHMENT_ID";
                    client.DeleteAttachment(attachmentId);
                    Console.WriteLine("Attachment deleted successfully.");
                }
                else
                {
                    Console.WriteLine("No attachments to delete.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
