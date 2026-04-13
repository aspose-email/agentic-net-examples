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
            // EWS connection settings
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define a new inbox rule
                InboxRule rule = new InboxRule
                {
                    DisplayName = "Add disclaimer for external recipients",
                    IsEnabled = true
                };

                // Set the action to reply with a template message (disclaimer)
                RuleActions actions = new RuleActions
                {
                    ServerReplyWithMessage = "TemplateMessageId" // ID of the disclaimer template
                };
                rule.Actions = actions;

                // Create the rule on the server
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
