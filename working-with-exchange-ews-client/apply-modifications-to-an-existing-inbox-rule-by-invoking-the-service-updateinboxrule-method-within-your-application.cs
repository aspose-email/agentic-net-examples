using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace UpdateInboxRuleSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Connection parameters – replace with real values.
                string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client. IEWSClient implements IDisposable.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Prepare the rule to be updated.
                    InboxRule rule = new InboxRule();
                    rule.RuleId = "YOUR_RULE_ID";               // Existing rule identifier.
                    rule.DisplayName = "Updated Rule Name";     // New display name.
                    rule.IsEnabled = true;                      // Enable the rule.
                    // Additional modifications can be made here, e.g., rule.Priority = 1;

                    // Update the rule on the server.
                    client.UpdateInboxRule(rule);

                    Console.WriteLine("Inbox rule updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
