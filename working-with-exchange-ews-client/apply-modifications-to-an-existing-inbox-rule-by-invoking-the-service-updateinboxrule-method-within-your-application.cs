using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define connection parameters
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                string userName = "user@example.com";
                string password = "password";

                // Create network credentials (avoid naming conflict with any existing variable)
                NetworkCredential networkCredential = new NetworkCredential(userName, password);

                // Initialize the EWS client inside a using block for proper disposal
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, networkCredential))
                {
                    // Create a new inbox rule
                    InboxRule rule = new InboxRule();
                    rule.DisplayName = "Sample Rule";
                    rule.IsEnabled = true;
                    // Additional rule configuration can be added here (conditions, actions, etc.)

                    // Update the rule on the server
                    ewsClient.UpdateInboxRule(rule);
                    Console.WriteLine("Inbox rule updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}