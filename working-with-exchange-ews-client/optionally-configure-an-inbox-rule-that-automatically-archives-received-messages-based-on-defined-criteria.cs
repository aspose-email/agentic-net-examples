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
            // Placeholder connection details – replace with real values for actual execution
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping rule creation.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define the sender whose messages should be archived
                MailAddress fromAddress = new MailAddress("sender@example.com");

                // Destination folder identifier (e.g., Archive folder). Replace with actual folder ID if needed.
                string archiveFolderId = "Archive";

                // Build the inbox rule
                InboxRule rule = InboxRule.CreateRuleMoveFrom(fromAddress, archiveFolderId);
                rule.DisplayName = "Archive messages from specific sender";
                rule.IsEnabled = true;
                rule.Priority = 1;

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
