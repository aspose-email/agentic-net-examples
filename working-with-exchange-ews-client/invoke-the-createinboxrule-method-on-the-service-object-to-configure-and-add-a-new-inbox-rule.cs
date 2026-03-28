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
            // Service URL and credentials (replace with actual values)
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("user@example.com", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Define a rule that moves messages from a specific sender to a target folder
                MailAddress sender = new MailAddress("spam@example.com");
                string targetFolder = "Inbox/Spam";

                InboxRule rule = InboxRule.CreateRuleMoveFrom(sender, targetFolder);
                rule.DisplayName = "Move spam to Spam folder";
                rule.IsEnabled = true;

                // Add the rule to the mailbox
                client.CreateInboxRule(rule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
