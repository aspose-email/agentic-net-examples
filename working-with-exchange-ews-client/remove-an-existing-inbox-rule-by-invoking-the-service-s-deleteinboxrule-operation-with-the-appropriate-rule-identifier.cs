using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – in real scenarios replace with actual values.
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing live calls with placeholder data.
            if (serviceUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping DeleteInboxRule operation.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Identifier of the inbox rule to delete.
                string ruleId = "rule-id-placeholder";

                try
                {
                    client.DeleteInboxRule(ruleId);
                    Console.WriteLine($"Inbox rule with ID '{ruleId}' deleted successfully.");
                }
                catch (AsposeArgumentException ex)
                {
                    Console.Error.WriteLine($"Failed to delete inbox rule: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
