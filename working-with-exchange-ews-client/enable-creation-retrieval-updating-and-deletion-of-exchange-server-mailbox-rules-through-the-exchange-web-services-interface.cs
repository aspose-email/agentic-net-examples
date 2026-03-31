using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsInboxRuleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Placeholder connection information
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Skip real network calls when placeholders are detected
                if (mailboxUri.Contains("example.com") ||
                    username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                    password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                    return;
                }

                // Client connection safety guard
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // -------------------- Create a new inbox rule --------------------
                        InboxRule newRule = new InboxRule();
                        newRule.DisplayName = "Sample Rule - Move from Specific Sender";
                        newRule.IsEnabled = true;
                        newRule.Priority = 1;

                        // Define conditions
                        RulePredicates conditions = new RulePredicates();
                        conditions.FromAddresses.Add(new MailAddress("sender@example.com"));
                        newRule.Conditions = conditions;

                        // Define actions (move matching messages to a folder)
                        RuleActions actions = new RuleActions();
                        actions.MoveToFolder = "Inbox/TargetFolder"; // Folder identifier or name
                        newRule.Actions = actions;

                        client.CreateInboxRule(newRule);
                        Console.WriteLine("Inbox rule created: " + newRule.DisplayName);

                        // -------------------- Retrieve existing inbox rules --------------------
                        InboxRule[] existingRules = client.GetInboxRules();
                        Console.WriteLine("Current inbox rules:");
                        foreach (InboxRule rule in existingRules)
                        {
                            Console.WriteLine($"- Id: {rule.RuleId}, Name: {rule.DisplayName}, Enabled: {rule.IsEnabled}");
                        }

                        // -------------------- Update the rule we just created --------------------
                        // Find the rule by display name
                        InboxRule ruleToUpdate = null;
                        foreach (InboxRule rule in existingRules)
                        {
                            if (rule.DisplayName == newRule.DisplayName)
                            {
                                ruleToUpdate = rule;
                                break;
                            }
                        }

                        if (ruleToUpdate != null)
                        {
                            ruleToUpdate.IsEnabled = false; // Disable the rule
                            client.UpdateInboxRule(ruleToUpdate);
                            Console.WriteLine("Inbox rule updated (disabled): " + ruleToUpdate.DisplayName);
                        }
                        else
                        {
                            Console.WriteLine("Created rule not found for update.");
                        }

                        // -------------------- Delete the rule --------------------
                        if (ruleToUpdate != null)
                        {
                            client.DeleteInboxRule(ruleToUpdate.RuleId);
                            Console.WriteLine("Inbox rule deleted: " + ruleToUpdate.DisplayName);
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine("EWS client error: " + clientEx.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}
