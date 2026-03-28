using System;
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
            // Initialize token provider (replace with real credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId", "clientSecret", "refreshToken");

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                try
                {
                    // Retrieve inbox rules
                    List<InboxRule> rules = client.ListRules();

                    Console.WriteLine("Inbox Rules:");
                    foreach (InboxRule rule in rules)
                    {
                        Console.WriteLine($"- {rule.DisplayName}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving rules: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Initialization error: {ex.Message}");
        }
    }
}
