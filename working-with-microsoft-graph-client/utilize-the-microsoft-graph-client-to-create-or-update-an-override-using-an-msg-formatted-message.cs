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
            // File path for the MSG message
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


            // Load the MSG message
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Extract the sender address
                MailAddress sender = message.From;

                // Create a classification override (e.g., classify as Focused)
                ClassificationOverride classificationOverride = new ClassificationOverride(sender, ClassificationType.Focused);

                // Prepare token provider (replace placeholders with real values)
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Tenant identifier (replace with actual tenant ID)
                string tenantId = "tenantId";

                // Initialize Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Create or update the override
                    ClassificationOverride result = client.CreateOrUpdateOverride(classificationOverride);
                    Console.WriteLine($"Override created/updated. ID: {result.Id}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
