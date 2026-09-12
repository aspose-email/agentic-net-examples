using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace DeleteInboxRuleSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS service URL and credentials
                string serviceUrl = "https://your-ews-url/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Identifier of the inbox rule to delete
                string ruleId = "rule-id-to-delete";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Delete the specified inbox rule
                        client.DeleteInboxRule(ruleId);
                        Console.WriteLine("Inbox rule deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete inbox rule: {ex.Message}");
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
