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
            // Initialize EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Create a new inbox rule that deletes messages from a specific sender
                InboxRule rule = InboxRule.CreateRuleDeleteFrom(new MailAddress("spam@example.com"));
                rule.DisplayName = "Delete Spam Sender";
                rule.IsEnabled = true;
                rule.Priority = 1;

                // Add the rule to the mailbox
                client.CreateInboxRule(rule);

                // Retrieve and display all inbox rules
                InboxRule[] rules = client.GetInboxRules();
                Console.WriteLine("Inbox Rules:");
                foreach (InboxRule r in rules)
                {
                    Console.WriteLine($"- {r.DisplayName} (Enabled: {r.IsEnabled})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
