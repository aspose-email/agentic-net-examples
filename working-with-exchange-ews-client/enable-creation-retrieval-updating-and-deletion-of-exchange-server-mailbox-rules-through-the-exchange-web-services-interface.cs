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
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new inbox rule that moves messages from a specific sender to a folder
                InboxRule newRule = InboxRule.CreateRuleMoveFrom(new MailAddress("news@example.com"), "Newsletters");
                newRule.DisplayName = "Move newsletters to folder";
                newRule.IsEnabled = true;

                // Create the rule on the server
                client.CreateInboxRule(newRule);
                Console.WriteLine("Inbox rule created.");

                // Retrieve all inbox rules
                InboxRule[] rules = client.GetInboxRules();
                Console.WriteLine("Current Inbox Rules:");
                foreach (InboxRule rule in rules)
                {
                    Console.WriteLine($"- {rule.DisplayName} (Enabled: {rule.IsEnabled})");
                }

                // Update the rule (change its display name)
                foreach (InboxRule rule in rules)
                {
                    if (rule.DisplayName == "Move newsletters to folder")
                    {
                        rule.DisplayName = "Move newsletters to Newsletters folder";
                        client.UpdateInboxRule(rule);
                        Console.WriteLine("Inbox rule updated.");
                        break;
                    }
                }

                // Delete the rule
                foreach (InboxRule rule in rules)
                {
                    if (rule.DisplayName == "Move newsletters to Newsletters folder")
                    {
                        client.DeleteInboxRule(rule.RuleId);
                        Console.WriteLine("Inbox rule deleted.");
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
