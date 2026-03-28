using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace placeholders with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                try
                {
                    // Create a rule that moves messages from a specific sender to the Inbox folder
                    MailAddress sender = new MailAddress("sender@example.com");
                    string destinationFolderId = client.MailboxInfo.InboxUri; // target folder

                    InboxRule rule = InboxRule.CreateRuleMoveFrom(sender, destinationFolderId);
                    rule.DisplayName = "Move messages from sender@example.com to Inbox";
                    rule.IsEnabled = true;

                    // Add the rule to the mailbox
                    client.CreateInboxRule(rule);
                    Console.WriteLine("Inbox rule created successfully.");

                    // List existing inbox rules
                    InboxRule[] existingRules = client.GetInboxRules();
                    Console.WriteLine("Current inbox rules:");
                    foreach (InboxRule r in existingRules)
                    {
                        Console.WriteLine($"- {r.DisplayName} (Enabled: {r.IsEnabled})");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while configuring inbox rules: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
