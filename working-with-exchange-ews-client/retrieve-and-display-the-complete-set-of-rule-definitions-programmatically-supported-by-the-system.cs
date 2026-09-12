using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

// Author: Aspose.Email example – retrieve and display all inbox rule definitions
class Program
{
    static void Main(string[] args)
    {
        // Replace with your actual EWS service URL and credentials
        string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";


        // Skip external calls when placeholder credentials are used
        if (username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password);
            // Get all inbox rules for the default mailbox (null or empty string)
            InboxRule[] rules = client.GetInboxRules(null);

            Console.WriteLine($"Total rules: {rules.Length}");
            foreach (InboxRule rule in rules)
            {
                Console.WriteLine($"Name: {rule.DisplayName}");
                Console.WriteLine($"Enabled: {rule.IsEnabled}");
                Console.WriteLine($"Priority: {rule.Priority}");
                Console.WriteLine(new string('-', 30));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
