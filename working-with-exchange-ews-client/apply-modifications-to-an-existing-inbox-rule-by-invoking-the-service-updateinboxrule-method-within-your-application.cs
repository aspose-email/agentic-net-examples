using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Retrieve existing inbox rules
                InboxRule[] inboxRules = client.GetInboxRules();

                // Find the rule to modify (by display name)
                InboxRule ruleToUpdate = null;
                foreach (InboxRule rule in inboxRules)
                {
                    if (rule.DisplayName == "Sample Rule")
                    {
                        ruleToUpdate = rule;
                        break;
                    }
                }

                if (ruleToUpdate == null)
                {
                    Console.WriteLine("Specified inbox rule not found.");
                    return;
                }

                // Example modification: disable the rule
                ruleToUpdate.IsEnabled = false;

                // Apply the update to the server
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
