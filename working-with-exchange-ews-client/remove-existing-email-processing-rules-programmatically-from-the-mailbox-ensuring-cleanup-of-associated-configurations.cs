using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder mailbox URI and credentials.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing real network calls with placeholder data.
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create and use the EWS client.
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Retrieve all inbox rules.
                        InboxRule[] inboxRules = client.GetInboxRules();

                        if (inboxRules == null || inboxRules.Length == 0)
                        {
                            Console.WriteLine("No inbox rules found.");
                        }
                        else
                        {
                            foreach (InboxRule rule in inboxRules)
                            {
                                if (rule != null && !string.IsNullOrEmpty(rule.RuleId))
                                {
                                    try
                                    {
                                        client.DeleteInboxRule(rule.RuleId);
                                        Console.WriteLine($"Deleted rule: {rule.DisplayName ?? "(no name)"}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine($"Failed to delete rule '{rule.DisplayName}': {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
