using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace UpdateInboxRuleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string serviceUrl = "https://example.com/EWS";
                string username = "username";
                string password = "password";
                string domain = "";

                // Guard against executing with placeholder credentials.
                if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping inbox rule update.");
                    return;
                }

                // Create the EWS client.
                using (IEWSClient service = EWSClient.GetEWSClient(serviceUrl, username, password, domain))
                {
                    try
                    {
                        // Retrieve existing inbox rules.
                        InboxRule[] existingRules = service.GetInboxRules();

                        if (existingRules == null || existingRules.Length == 0)
                        {
                            Console.Error.WriteLine("No inbox rules found to update.");
                            return;
                        }

                        // Select the first rule for demonstration purposes.
                        InboxRule ruleToUpdate = existingRules[0];

                        // Modify the rule – for example, toggle its enabled state and change the display name.
                        ruleToUpdate.IsEnabled = !ruleToUpdate.IsEnabled;
                        ruleToUpdate.DisplayName = ruleToUpdate.DisplayName + " (Updated)";

                        // Apply the update.
                        service.UpdateInboxRule(ruleToUpdate);

                        Console.WriteLine("Inbox rule updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while updating inbox rule: {ex.Message}");
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
}
