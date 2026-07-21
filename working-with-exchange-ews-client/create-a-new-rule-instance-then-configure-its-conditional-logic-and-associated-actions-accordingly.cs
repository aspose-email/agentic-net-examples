using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main()
        {
            // Exchange Web Services (EWS) client configuration
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client inside a using block to ensure proper disposal
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Define a rule that moves messages from a specific sender to a target folder
                    MailAddress fromAddress = new MailAddress("sender@example.com");
                    string destinationFolderId = "Inbox/Invoices";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    InboxRule rule = InboxRule.CreateRuleMoveFrom(fromAddress, destinationFolderId);
                    rule.DisplayName = "Move invoices from sender";
                    rule.IsEnabled = true;

                    // Create the rule on the default mailbox
                    client.CreateInboxRule(rule);

                    Console.WriteLine("Inbox rule created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // In a real application, consider logging the exception details.
            }
        }
    }
}
