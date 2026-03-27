using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace ExchangeInboxRuleSample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Exchange Web Services endpoint and credentials (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // -------------------- Create a new inbox rule --------------------
                    InboxRule newRule = new InboxRule();
                    newRule.DisplayName = "Sample Rule";
                    newRule.IsEnabled = true;

                    client.CreateInboxRule(newRule);
                    Console.WriteLine("Inbox rule created.");

                    // -------------------- Retrieve all inbox rules --------------------
                    InboxRule[] allRules = client.GetInboxRules();

                    Console.WriteLine("Current inbox rules:");
                    foreach (InboxRule rule in allRules)
                    {
                        Console.WriteLine($"- {rule.DisplayName} (Enabled: {rule.IsEnabled})");
                    }

                    // Find the rule we just created (by display name)
                    InboxRule targetRule = null;
                    foreach (InboxRule rule in allRules)
                    {
                        if (rule.DisplayName == "Sample Rule")
                        {
                            targetRule = rule;
                            break;
                        }
                    }

                    if (targetRule != null)
                    {
                        // -------------------- Update the inbox rule --------------------
                        targetRule.DisplayName = "Updated Sample Rule";
                        client.UpdateInboxRule(targetRule);
                        Console.WriteLine("Inbox rule updated.");

                        // -------------------- Delete the inbox rule --------------------
                        client.DeleteInboxRule(targetRule.RuleId);
                        Console.WriteLine("Inbox rule deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Created rule not found for update/delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}