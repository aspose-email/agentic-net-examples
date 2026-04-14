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
            // Placeholder values – replace with real server details.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string messageUri = "https://exchange.example.com/EWS/MessageId";

            // Skip execution when placeholders are detected.
            if (mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve the original message.
                MailMessage originalMessage = client.FetchMessage(messageUri);

                // Determine the sender address.
                MailAddress senderAddress = null;
                if (originalMessage.From != null && originalMessage.From.Count > 0)
                {
                    senderAddress = originalMessage.From[0];
                }

                // Build a server‑side rule to forward messages from this sender.
                InboxRule rule = new InboxRule
                {
                    DisplayName = "Forward from specific sender",
                    IsEnabled = true
                };

                // Condition: messages from the identified sender.
                if (senderAddress != null)
                {
                    rule.Conditions.FromAddresses.Add(senderAddress);
                }

                // Action: forward to another address.
                rule.Actions.ForwardToRecipients.Add(new MailAddress("forwardto@example.com"));

                // Apply the rule on the Exchange server.
                client.CreateInboxRule(rule);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
