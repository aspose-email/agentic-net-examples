using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected to avoid runtime failures.
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Validate connectivity by attempting to access the Inbox folder.
                try
                {
                    var inboxInfo = client.GetFolderInfo(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection validation failed: {ex.Message}");
                    return;
                }

                // Obtain the Drafts folder URI.
                string draftsFolderUri = client.MailboxInfo.DraftsUri;

                // List messages from Drafts with a custom page size of 50.
                int pageSize = 50;
                ExchangeMessageInfoCollection messages = client.ListMessages(draftsFolderUri, pageSize);

                Console.WriteLine($"Retrieved {messages.Count} message(s) from Drafts (max {pageSize} per request).");
                foreach (var msgInfo in messages)
                {
                    Console.WriteLine($"Subject: {msgInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
