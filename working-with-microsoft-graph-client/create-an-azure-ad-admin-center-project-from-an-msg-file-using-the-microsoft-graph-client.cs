using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the MSG file exists
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


            // Load the MSG file into a MailMessage object
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                return;
            }

            // Create a token provider for Microsoft Graph authentication
            Aspose.Email.Clients.TokenProvider tokenProvider;
            try
            {
                tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",          // Replace with your Azure AD app client ID
                    "clientSecret",      // Replace with your Azure AD app client secret
                    "refreshToken");     // Replace with a valid refresh token
            }
            catch (Exception tokenEx)
            {
                Console.Error.WriteLine($"Token provider initialization failed: {tokenEx.Message}");
                message.Dispose();
                return;
            }

            // Initialize the Graph client
            IGraphClient client;
            try
            {
                client = GraphClient.GetClient(tokenProvider, "tenantId"); // Replace with your tenant ID
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Graph client creation failed: {clientEx.Message}");
                message.Dispose();
                return;
            }

            // Use the client to create the message in the Inbox folder
            using (client)
            {
                try
                {
                    client.CreateMessage("Inbox", message);
                    Console.WriteLine("Message uploaded to Azure AD Admin Center project successfully.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create message: {createEx.Message}");
                }
            }

            // Dispose the MailMessage
            message.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
