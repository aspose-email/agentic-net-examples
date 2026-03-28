using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source MSG file
            string msgPath = "source.msg";
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Destination folder identifier in the target mailbox (Graph folder ItemId)
            string destinationFolderId = "DESTINATION_FOLDER_ITEM_ID";

            // Load the MSG file into a MailMessage object
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Initialize token provider (dummy credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Create Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
                {
                    try
                    {
                        // Create the message in the destination folder
                        client.CreateMessage(destinationFolderId, message);
                        Console.WriteLine("Message copied successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during message copy: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
