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
            string msgFilePath = "sample.msg";
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = new MailAddress("sender@example.com");
                        placeholder.To.Add(new MailAddress("recipient@example.com"));
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "Placeholder body.";
                        placeholder.Save(msgFilePath, SaveOptions.DefaultMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder MSG file: " + ex.Message);
                    return;
                }
            }

            // Load the MSG file
            MailMessage loadedMessage;
            try
            {
                loadedMessage = MailMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to load MSG file: " + ex.Message);
                return;
            }

            // Extract sender address
            string senderAddress = loadedMessage.From.Address;

            // Dispose the loaded message
            loadedMessage.Dispose();

            // Create an inbox rule that moves messages from the sender to a folder named "Processed"
            InboxRule rule = InboxRule.CreateRuleMoveFrom(new MailAddress(senderAddress), "Processed");

            // Initialize token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create token provider: " + ex.Message);
                return;
            }

            // Create Graph client and add the rule
            try
            {
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    InboxRule createdRule = graphClient.CreateRule(rule);
                    Console.WriteLine("Rule created with ID: " + createdRule.RuleId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Graph client operation failed: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}