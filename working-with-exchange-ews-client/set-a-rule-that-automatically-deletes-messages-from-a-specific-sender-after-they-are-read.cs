using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string mailboxUri = "https://mail.example.com/exchange/username";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Sender whose messages should be deleted after being read
            MailAddress senderAddress = new MailAddress("spam@example.com");

            // Create an inbox rule that deletes messages from the specified sender
            InboxRule deleteRule = InboxRule.CreateRuleDeleteFrom(senderAddress);
            deleteRule.IsEnabled = true; // Ensure the rule is active

            // Connect to the Exchange server using IEWSClient (WebDAV)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new System.Net.NetworkCredential(username, password)))
            {
                // Add the rule to the mailbox
                client.CreateInboxRule(deleteRule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
