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
            // Mailbox connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve existing inbox rules
                InboxRule[] rules = client.GetInboxRules();

                Console.WriteLine($"Found {rules.Length} inbox rule(s).");

                // Delete each rule
                foreach (InboxRule rule in rules)
                {
                    try
                    {
                        client.DeleteInboxRule(rule.RuleId);
                        Console.WriteLine($"Deleted rule: {rule.DisplayName}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete rule '{rule.DisplayName}': {ex.Message}");
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
