using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace with actual service URL and credentials)
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Fetch all inbox rules for the default mailbox
                    InboxRule[] rules = client.GetInboxRules();

                    Console.WriteLine($"Total inbox rules: {rules.Length}");
                    foreach (InboxRule rule in rules)
                    {
                        Console.WriteLine($"- Rule: {rule.DisplayName}");
                        Console.WriteLine($"  Enabled: {rule.IsEnabled}");
                        Console.WriteLine($"  Priority: {rule.Priority}");
                        // Additional properties can be inspected as needed
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving inbox rules: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to initialize EWS client: {ex.Message}");
        }
    }
}
