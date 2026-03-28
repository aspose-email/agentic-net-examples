using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Clients.Exchange;

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


            // Load the message (MailMessage implements IDisposable)
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Extract sender address for rule creation
                MailAddress sender = message.From;

                // Destination folder identifier (replace with a real folder ItemId)
                string destinationFolderId = "folderId";

                // Create an inbox rule that moves messages from the sender to the folder
                InboxRule rule = InboxRule.CreateRuleMoveFrom(sender, destinationFolderId);
                rule.DisplayName = "Move messages from " + sender.Address;
                rule.IsEnabled = true;

                // Prepare token provider (replace with real credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Initialize Graph client
                using (IGraphClient graphClient = Aspose.Email.Clients.Graph.GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
                {
                    // Create the rule in the user's inbox
                    InboxRule createdRule = graphClient.CreateRule(rule);
                    Console.WriteLine($"Rule created with ID: {createdRule.RuleId}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
