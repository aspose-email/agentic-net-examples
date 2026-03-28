using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace placeholders with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://example.com/EWS/Exchange.asmx", "username", "password"))
            {
                try
                {
                    // Retrieve all inbox rules
                    InboxRule[] rules = client.GetInboxRules();

                    // Display rule details
                    foreach (InboxRule rule in rules)
                    {
                        Console.WriteLine($"Name: {rule.DisplayName}");
                        Console.WriteLine($"Enabled: {rule.IsEnabled}");
                        Console.WriteLine($"Priority: {rule.Priority}");
                        Console.WriteLine($"Rule ID: {rule.RuleId}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving inbox rules: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
