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

            // Verify the file exists before attempting to load
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


            // Load the MSG file
            using (MailMessage msg = MailMessage.Load(msgPath))
            {
                // Use the subject of the message as the category name (fallback to a default name)
                string categoryName = string.IsNullOrEmpty(msg.Subject) ? "DefaultCategory" : msg.Subject;

                // Create a token provider (replace placeholders with real values)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",      // Client ID
                    "clientSecret",  // Client Secret
                    "refreshToken"   // Refresh Token
                );

                // Initialize the Microsoft Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
                {
                    // Create a new Outlook category using a preset color
                    OutlookCategory createdCategory = client.CreateCategory(
                        categoryName,
                        CategoryPreset.Preset0   // Use a valid preset enum value
                    );

                    Console.WriteLine($"Created category: {createdCategory.DisplayName}, Preset: {createdCategory.Preset}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
