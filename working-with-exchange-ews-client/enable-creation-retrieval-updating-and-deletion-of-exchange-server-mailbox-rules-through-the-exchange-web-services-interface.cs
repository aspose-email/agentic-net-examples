using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (connection safety)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                try
                {
                    // -------------------- Create a new inbox rule --------------------
                    InboxRule newRule = new InboxRule();
                    newRule.DisplayName = "Sample Rule";
                    newRule.IsEnabled = true;
                    client.CreateInboxRule(newRule);
                    Console.WriteLine("Inbox rule created.");

                    // -------------------- Retrieve existing inbox rules --------------------
                    InboxRule[] rules = client.GetInboxRules();
                    Console.WriteLine("Existing inbox rules:");
                    foreach (InboxRule rule in rules)
                    {
                        Console.WriteLine($"- {rule.DisplayName} (Id: {rule.RuleId})");
                    }

                    // -------------------- Update the created rule (disable it) --------------------
                    InboxRule ruleToUpdate = null;
                    foreach (InboxRule rule in rules)
                    {
                        if (rule.DisplayName == "Sample Rule")
                        {
                            ruleToUpdate = rule;
                            break;
                        }
                    }

                    if (ruleToUpdate != null)
                    {
                        ruleToUpdate.IsEnabled = false;
                        client.UpdateInboxRule(ruleToUpdate);
                        Console.WriteLine("Inbox rule updated (disabled).");
                    }

                    // -------------------- Delete the rule --------------------
                    if (ruleToUpdate != null)
                    {
                        client.DeleteInboxRule(ruleToUpdate.RuleId);
                        Console.WriteLine("Inbox rule deleted.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors from EWS operations
                    Console.Error.WriteLine($"EWS operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
