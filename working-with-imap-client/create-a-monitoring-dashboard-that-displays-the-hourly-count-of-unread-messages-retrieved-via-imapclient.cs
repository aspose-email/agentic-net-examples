using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping connection.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder(ImapFolderInfo.InBox);

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    // Count unread messages
                    int unreadCount = 0;
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        if (!messageInfo.IsRead)
                        {
                            unreadCount++;
                        }
                    }

                    // Display the hourly unread count (for demonstration we use current hour)
                    DateTime now = DateTime.Now;
                    Console.WriteLine($"[{now:yyyy-MM-dd HH:00}] Unread messages: {unreadCount}");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
