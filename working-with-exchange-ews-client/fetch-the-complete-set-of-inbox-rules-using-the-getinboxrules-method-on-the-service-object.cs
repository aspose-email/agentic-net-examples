using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailInboxRulesSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize the EWS client inside a nested try/catch to handle connection issues.
                try
                {
                    // Replace the service URL, username, and password with valid credentials.
                    using (IEWSClient client = EWSClient.GetEWSClient(
                        "https://exchange.example.com/EWS/Exchange.asmx",
                        new NetworkCredential("username", "password")))
                    {
                        // Retrieve all inbox rules for the default mailbox.
                        InboxRule[] inboxRules = client.GetInboxRules();

                        Console.WriteLine($"Total inbox rules: {inboxRules.Length}");
                        foreach (InboxRule rule in inboxRules)
                        {
                            Console.WriteLine($"Rule: {rule.DisplayName}, Enabled: {rule.IsEnabled}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error initializing or using EWS client: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
