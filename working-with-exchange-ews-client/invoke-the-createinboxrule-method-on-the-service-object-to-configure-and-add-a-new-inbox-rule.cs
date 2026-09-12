using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailInboxRuleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder credentials – replace with real values for actual execution.
            const string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            const string username = "user@example.com";
            const string password = "password";

            // Guard: skip network operations when placeholders are detected.
            bool isPlaceholder = username.Contains("@example.com") || password.Equals("password", StringComparison.OrdinalIgnoreCase);
            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping creation of inbox rule.");
                return;
            }

            try
            {
                // Initialize EWS client (service) with real credentials.
                IEWSClient service = EWSClient.GetEWSClient(mailboxUri, username, password);

                // Build an inbox rule that deletes messages from a specific sender.
                MailAddress unwantedSender = new MailAddress("spam@example.com");
                InboxRule rule = InboxRule.CreateRuleDeleteFrom(unwantedSender);
                rule.DisplayName = "Delete spam from spam@example.com";
                rule.IsEnabled = true;
                rule.Priority = 1; // Run early

                // Create the rule in the default mailbox.
                service.CreateInboxRule(rule);

                Console.WriteLine("Inbox rule created successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Gracefully exit without rethrowing.
            }
        }
    }
}
