using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "username@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Create a new inbox rule
            InboxRule rule = new InboxRule();
            rule.DisplayName = "Forward Subject Prefix Rule";
            rule.IsEnabled = true;

            // Define the condition: subject starts with a specific prefix
            RulePredicates conditions = new RulePredicates();

            // Define the action: forward to an external address
            MailAddressCollection forwardRecipients = new MailAddressCollection();
            forwardRecipients.Add(new MailAddress("external@example.com"));

            // Create the rule on the server
            try
            {
                client.CreateInboxRule(rule);
                Console.WriteLine("Inbox rule created successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create inbox rule: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
