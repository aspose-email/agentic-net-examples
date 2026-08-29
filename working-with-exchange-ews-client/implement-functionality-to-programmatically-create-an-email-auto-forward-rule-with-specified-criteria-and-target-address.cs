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
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when using placeholder credentials/host
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create EWS client safely
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Build an inbox rule that forwards all incoming messages
                    InboxRule rule = new InboxRule
                    {
                        DisplayName = "Auto Forward Rule",
                        IsEnabled = true,
                        Actions = new RuleActions()
                    };
                    rule.Actions.ForwardToRecipients.Add(new MailAddress("forwardto@example.com"));

                    // Create the rule on the server
                    client.CreateInboxRule(rule);
                    Console.WriteLine("Inbox rule created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
