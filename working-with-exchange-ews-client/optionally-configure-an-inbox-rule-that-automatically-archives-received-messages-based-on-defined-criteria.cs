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
            // Author note: Adjust the following credentials and service URL to match your Exchange environment.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define criteria for messages to be archived (e.g., subject contains "Invoice").
                string[] containingStrings = new[] { "Invoice" };

                // Destination folder identifier where messages will be moved.
                // Replace with the actual folder ID of the Archive folder in your mailbox.
                string archiveFolderId = "Archive";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create an inbox rule that moves matching messages to the archive folder.
                InboxRule archiveRule = InboxRule.CreateRuleMoveContaining(containingStrings, archiveFolderId);
                archiveRule.DisplayName = "Archive Invoices";
                archiveRule.IsEnabled = true;

                // Create the rule in the default mailbox.
                client.CreateInboxRule(archiveRule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
