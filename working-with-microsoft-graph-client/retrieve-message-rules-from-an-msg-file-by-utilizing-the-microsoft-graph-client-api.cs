using System;
using System.IO;
using System.Collections.Generic;
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
            // Ensure the MSG file exists; create a minimal placeholder if missing
            string msgPath = "sample.msg";
            if (!File.Exists(msgPath))
            {
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "Body"))
                {
                    placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                }
            }

            // Placeholder credentials for Outlook token provider
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Skip actual network call when using placeholder credentials
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph client call.");
                return;
            }

            // Obtain token provider for Microsoft Graph
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Create Graph client and retrieve inbox rules
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                List<InboxRule> rules = client.ListRules();
                Console.WriteLine($"Retrieved {rules.Count} inbox rule(s):");
                foreach (InboxRule rule in rules)
                {
                    Console.WriteLine($"- {rule.DisplayName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
