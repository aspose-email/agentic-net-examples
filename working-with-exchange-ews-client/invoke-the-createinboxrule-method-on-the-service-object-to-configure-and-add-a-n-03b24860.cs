using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define the sender to block
                MailAddress sender = new MailAddress("spam@example.com");

                // Create a rule that deletes messages from the specified sender
                InboxRule rule = InboxRule.CreateRuleDeleteFrom(sender);
                rule.DisplayName = "Delete spam from specific sender";
                rule.IsEnabled = true;

                // Add the rule to the mailbox
                client.CreateInboxRule(rule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}