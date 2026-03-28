using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // IMAP server connection parameters (replace with real values or keep placeholders)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                SecurityOptions security = SecurityOptions.SSLExplicit;

                // Create and connect the IMAP client inside a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, port, username, password, security))
                {
                    try
                    {
                        // List messages in the INBOX folder
                        ImapMessageInfoCollection messagesInfo = client.ListMessages("INBOX");

                        Console.WriteLine($"Total messages in INBOX: {messagesInfo.Count}");

                        // Iterate through each message info, fetch the full message and display its subject
                        foreach (ImapMessageInfo info in messagesInfo)
                        {
                            // Fetch the full MailMessage using the unique identifier (UID)
                            MailMessage message = client.FetchMessage(info.UniqueId);
                            string subject = message.Subject ?? string.Empty;
                            Console.WriteLine($"Subject: {subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during mailbox operations
                        Console.Error.WriteLine($"Error accessing mailbox: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
