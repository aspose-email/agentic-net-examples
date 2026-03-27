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
            // Service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Retrieve existing inbox rules
                InboxRule[] rules = client.GetInboxRules();

                // Identify the rule to modify (by display name)
                string targetDisplayName = "Sample Rule";
                InboxRule ruleToUpdate = null;
                foreach (InboxRule rule in rules)
                {
                    if (rule.DisplayName == targetDisplayName)
                    {
                        ruleToUpdate = rule;
                        break;
                    }
                }

                if (ruleToUpdate == null)
                {
                    Console.Error.WriteLine("Inbox rule not found.");
                    return;
                }

                // Modify desired properties of the rule
                ruleToUpdate.IsEnabled = false; // Example: disable the rule

                // Apply the modification on the server
                client.UpdateInboxRule(ruleToUpdate);

                Console.WriteLine("Inbox rule updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
