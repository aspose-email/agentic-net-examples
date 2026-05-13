using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string host = "exchange.example.com";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping live Exchange connection.");
                    return;
                }

                // Create and connect the Exchange client.
                using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                {
                    // Keywords that identify spam messages.
                    string[] spamKeywords = new string[] { "spam", "phishing", "malware" };

                    // Identifier of the quarantine folder (must be a valid folder ID in the mailbox).
                    string quarantineFolderId = "quarantineFolderId";

                    // Build the inbox rule that moves matching messages to the quarantine folder.
                    InboxRule spamRule = InboxRule.CreateRuleMoveContaining(spamKeywords, quarantineFolderId);
                    spamRule.DisplayName = "Spam Filter to Quarantine";
                    spamRule.IsEnabled = true;

                    // Create the rule on the server.
                    client.CreateInboxRule(spamRule);

                    Console.WriteLine("Spam filter rule created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
