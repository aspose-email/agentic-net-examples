using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace placeholders with real values)
            IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx",
                                                       new NetworkCredential("username", "password"));
            using (client)
            {
                // Create a rule that deletes messages from a specific sender
                MailAddress sender = new MailAddress("spam@example.com");
                InboxRule deleteFromSenderRule = InboxRule.CreateRuleDeleteFrom(sender);
                deleteFromSenderRule.DisplayName = "Delete Spam Sender";
                deleteFromSenderRule.IsEnabled = true;
                deleteFromSenderRule.Priority = 1;

                // Apply the rule to the mailbox
                client.CreateInboxRule(deleteFromSenderRule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
