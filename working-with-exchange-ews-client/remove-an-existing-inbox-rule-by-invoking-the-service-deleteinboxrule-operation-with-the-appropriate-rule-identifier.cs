using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Identifier of the inbox rule to delete
                string ruleId = "your-rule-id";

                // Delete the specified inbox rule
                client.DeleteInboxRule(ruleId);
                Console.WriteLine("Inbox rule deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
