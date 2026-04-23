using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server details
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip actual connection when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Skipping IMAP connection due to placeholder credentials.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Retrieve information about the INBOX folder
                    ImapFolderInfo inboxInfo = client.GetFolderInfo(ImapFolderInfo.InBox);
                    int totalMessages = inboxInfo.TotalMessageCount;
                    Console.WriteLine($"Total messages in INBOX: {totalMessages}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing mailbox: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
