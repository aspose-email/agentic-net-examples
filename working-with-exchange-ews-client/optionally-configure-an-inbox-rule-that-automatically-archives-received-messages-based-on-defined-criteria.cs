using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with placeholder credentials
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx",
                                                             new NetworkCredential("username", "password")))
            {
                // Create an inbox rule that moves messages containing specific keywords to the Archive folder
                InboxRule rule = InboxRule.CreateRuleMoveContaining(
                    new string[] { "confidential", "archive" }, // keywords to match
                    "Archive"                                   // target folder name
                );

                // Configure additional rule properties
                rule.DisplayName = "Auto-Archive Confidential Messages";
                rule.IsEnabled = true;
                rule.Priority = 1; // Run this rule first

                // Create the rule on the server
                client.CreateInboxRule(rule);

                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
