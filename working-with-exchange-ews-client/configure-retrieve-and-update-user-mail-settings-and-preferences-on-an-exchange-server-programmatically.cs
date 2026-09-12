using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using System;

class Program
{
    static void Main()
    {
        // Placeholder values – replace with real credentials to run against an actual server.
        string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";
        string domain = "example.com";

        // Guard: skip network operations when placeholders are detected.
        bool placeholdersDetected = mailboxUri.Contains("example.com") ||
                                    username.Contains("example.com") ||
                                    password.Equals("password", StringComparison.OrdinalIgnoreCase) ||
                                    domain.Equals("example.com", StringComparison.OrdinalIgnoreCase);

        if (placeholdersDetected)
        {
            Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
            return;
        }

        try
        {
            // Initialize EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, domain))
            {
                // Retrieve mailbox information.
                var mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine("Mailbox URIs:");
                Console.WriteLine($"Inbox: {mailboxInfo.InboxUri}");
                Console.WriteLine($"Sent Items: {mailboxInfo.SentItemsUri}");
                Console.WriteLine($"Drafts: {mailboxInfo.DraftsUri}");
                Console.WriteLine($"Deleted Items: {mailboxInfo.DeletedItemsUri}");

                // Example: Update Out of Office (OOF) settings.
                // Note: This requires actual server access and appropriate permissions.
                // The following is a placeholder for OOF update logic.
                /*
                {
                };
                Console.WriteLine("Out of Office settings updated.");
                */
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
