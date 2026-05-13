using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "your_username";
            string password = "your_password";
            string domain = "";

            // Skip execution if placeholders are detected
            if (serviceUrl.Contains("example") || username.Contains("your_") || password.Contains("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping rule deletion.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password, domain))
            {
                // Identifier of the rule to delete
                string ruleId = "your_rule_id";

                if (string.IsNullOrEmpty(ruleId))
                {
                    Console.Error.WriteLine("Rule identifier is null or empty.");
                    return;
                }

                try
                {
                    client.DeleteInboxRule(ruleId);
                    Console.WriteLine($"Inbox rule with ID '{ruleId}' deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting rule: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
