using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

public class Program
{
    public static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Service URL and credentials (replace with actual values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client and ensure it is disposed properly
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                try
                {
                    // Fetch all inbox rules
                    InboxRule[] rules = client.GetInboxRules();

                    // Display each rule's details
                    foreach (InboxRule rule in rules)
                    {
                        Console.WriteLine("Rule Name: " + rule.DisplayName);
                        Console.WriteLine("Enabled: " + rule.IsEnabled);
                        Console.WriteLine("Priority: " + rule.Priority);
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors while retrieving rules
                    Console.Error.WriteLine("Error retrieving inbox rules: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors during client creation or other failures
            Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
        }
    }
}