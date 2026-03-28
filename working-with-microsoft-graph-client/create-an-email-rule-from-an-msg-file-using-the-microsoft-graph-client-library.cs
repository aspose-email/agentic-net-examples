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
            // Paths and credentials (replace with real values)
            string msgPath = "sample.msg";
            string tenantId = "your-tenant-id";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = new MailAddress("sender@example.com");
                        placeholder.To.Add(new MailAddress("recipient@example.com"));
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "Placeholder body.";
                        placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Extract sender address for rule condition
                string senderEmail = message.From?.Address ?? string.Empty;
                if (string.IsNullOrEmpty(senderEmail))
                {
                    Console.Error.WriteLine("Message does not contain a valid sender address.");
                    return;
                }

                // Define the target folder name where matching messages will be moved
                string targetFolderName = "TargetFolder";

                // Create an inbox rule that moves messages from the sender to the target folder
                InboxRule rule = InboxRule.CreateRuleMoveFrom(new MailAddress(senderEmail), targetFolderName);
                rule.DisplayName = $"Move messages from {senderEmail}";
                rule.IsEnabled = true;

                // Obtain an Outlook token provider
                Aspose.Email.Clients.ITokenProvider tokenProvider;
                try
                {
                    tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to obtain token provider: {ex.Message}");
                    return;
                }

                // Initialize the Graph client
                IGraphClient client;
                try
                {
                    client = GraphClient.GetClient(tokenProvider, tenantId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                    return;
                }

                using (client)
                {
                    try
                    {
                        // Create the rule on the server
                        InboxRule createdRule = client.CreateRule(rule);
                        Console.WriteLine($"Rule created successfully. Rule ID: {createdRule.RuleId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create rule: {ex.Message}");
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
