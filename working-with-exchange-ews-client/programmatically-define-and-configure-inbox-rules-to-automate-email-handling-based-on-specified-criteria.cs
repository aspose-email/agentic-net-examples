using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // List existing inbox rules
                    InboxRule[] existingRules = client.GetInboxRules();
                    Console.WriteLine($"Existing rules count: {existingRules.Length}");

                    // Define a new rule: move messages from a specific sender to the Inbox folder
                    MailAddress fromAddress = new MailAddress("sender@example.com");
                    string destinationFolderId = client.MailboxInfo.InboxUri; // Use Inbox as destination
                    InboxRule newRule = InboxRule.CreateRuleMoveFrom(fromAddress, destinationFolderId);
                    newRule.DisplayName = "Move from specific sender";
                    newRule.IsEnabled = true;

                    // Create the rule on the server
                    client.CreateInboxRule(newRule);
                    Console.WriteLine("New rule created.");

                    // Retrieve the rule back (by listing again) to demonstrate update
                    InboxRule[] updatedRules = client.GetInboxRules();
                    foreach (InboxRule rule in updatedRules)
                    {
                        if (rule.DisplayName == "Move from specific sender")
                        {
                            // Disable the rule as an example of update
                            rule.IsEnabled = false;
                            client.UpdateInboxRule(rule);
                            Console.WriteLine("Rule updated (disabled).");

                            // Delete the rule as cleanup
                            if (!string.IsNullOrEmpty(rule.RuleId))
                            {
                                client.DeleteInboxRule(rule.RuleId);
                                Console.WriteLine("Rule deleted.");
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
