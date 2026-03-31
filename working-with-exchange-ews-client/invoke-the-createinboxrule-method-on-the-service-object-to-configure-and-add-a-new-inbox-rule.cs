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

            // Guard against executing real network calls with placeholder data
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Define a new inbox rule: move messages from a specific sender to a folder
                    MailAddress fromAddress = new MailAddress("sender@example.com");
                    string destinationFolderId = client.MailboxInfo.InboxUri; // using Inbox as destination for demo

                    InboxRule rule = InboxRule.CreateRuleMoveFrom(fromAddress, destinationFolderId);
                    rule.DisplayName = "Move messages from sender@example.com";
                    rule.IsEnabled = true;

                    // Add the rule to the mailbox
                    client.CreateInboxRule(rule);

                    Console.WriteLine("Inbox rule created successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while creating inbox rule: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
