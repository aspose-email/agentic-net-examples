using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox connection settings (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the sender whose messages will be forwarded
                MailAddress fromAddress = new MailAddress("sender@example.com");

                // Destination folder ID where matching messages will be moved
                // (replace with an actual folder ID or use a well‑known folder name)
                string destinationFolderId = "Inbox/Forwarded";

                // Create a basic rule that moves messages from the specified sender
                InboxRule rule = InboxRule.CreateRuleMoveFrom(fromAddress, destinationFolderId);

                // Configure additional rule properties
                rule.DisplayName = "Auto‑Forward from sender@example.com";
                rule.IsEnabled = true;
                rule.Priority = 1; // Higher priority runs earlier

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