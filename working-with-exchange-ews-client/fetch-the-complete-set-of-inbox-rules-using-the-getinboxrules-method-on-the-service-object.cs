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
            // Service URL and credentials for the Exchange server
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client (IEWSClient) using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Retrieve all inbox rules
                InboxRule[] rules = client.GetInboxRules();

                // Display rule information
                foreach (InboxRule rule in rules)
                {
                    Console.WriteLine($"Rule Name: {rule.DisplayName}");
                    Console.WriteLine($"Enabled: {rule.IsEnabled}");
                    Console.WriteLine($"Priority: {rule.Priority}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
