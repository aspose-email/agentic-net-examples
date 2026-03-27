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
            // Connect to Exchange using EWS
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Create a new inbox rule
                InboxRule rule = new InboxRule();
                rule.DisplayName = "Auto Forward Rule";
                rule.IsEnabled = true;

                // Define the action: forward incoming messages to a specific address
                RuleActions actions = new RuleActions();
                actions.ForwardToRecipients = new MailAddressCollection();
                actions.ForwardToRecipients.Add(new MailAddress("forward@example.com"));
                rule.Actions = actions;

                // No specific conditions are set; the rule applies to all incoming messages

                // Create the rule on the server
                client.CreateInboxRule(rule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
