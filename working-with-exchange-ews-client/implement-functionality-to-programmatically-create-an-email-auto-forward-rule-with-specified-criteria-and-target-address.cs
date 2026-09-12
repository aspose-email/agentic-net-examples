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
            // Placeholder values – replace with real credentials or skip execution.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping rule creation.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build the rule.
                InboxRule rule = new InboxRule
                {
                    DisplayName = "Auto‑Forward Rule",
                    IsEnabled = true,
                    Conditions = new RulePredicates(),
                    Actions = new RuleActions()
                };

                // Condition: messages from a specific sender.
                rule.Conditions.FromAddresses.Add(new MailAddress("sender@domain.com"));

                // Action: forward matching messages to the target address.
                rule.Actions.ForwardToRecipients.Add(new MailAddress("forwardto@domain.com"));

                // Create the rule on the server.
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
