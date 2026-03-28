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
            // Create a token provider with dummy credentials.
            // Replace the placeholder strings with real values when running the code.
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Initialize the Microsoft Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
            {
                // IDs of the message and the attachment to be removed.
                // Replace these with actual IDs from your mailbox.
                string messageId = "MESSAGE_ID";
                string attachmentId = "ATTACHMENT_ID";

                // Remove the attachment from the specified message.
                client.DeleteAttachment(attachmentId);
                Console.WriteLine("Attachment removed from the message.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors without crashing the application.
            Console.Error.WriteLine(ex.Message);
        }
    }
}
