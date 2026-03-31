using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (string.IsNullOrWhiteSpace(serviceUrl) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Fetch all inbox rules.
                    InboxRule[] rules = client.GetInboxRules();

                    Console.WriteLine($"Total inbox rules: {rules.Length}");
                    foreach (InboxRule rule in rules)
                    {
                        Console.WriteLine($"- {rule.DisplayName}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while retrieving inbox rules: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
