using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace placeholders with real values)
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                // Example rule identifier to delete (replace with actual rule IDs)
                string[] ruleIdsToDelete = new string[] { "rule-id-1", "rule-id-2" };

                foreach (string ruleId in ruleIdsToDelete)
                {
                    try
                    {
                        client.DeleteInboxRule(ruleId);
                        Console.WriteLine($"Deleted rule with ID: {ruleId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete rule {ruleId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
