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
            // EWS service URL and credentials
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string userName = "user@example.com";
            string userPassword = "password";

            // Identifier of the inbox rule to delete
            string ruleId = "rule-id-to-delete";

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(userName, userPassword)))
                {
                    // Delete the specified inbox rule
                    client.DeleteInboxRule(ruleId);
                    Console.WriteLine("Inbox rule deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
