using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Base;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Skipping IMAP connection due to placeholder credentials.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient imapClient = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    imapClient.Username = username;
                    imapClient.Password = password;

                    // Select the INBOX folder
                    imapClient.SelectFolder("INBOX");

                    // Retrieve list of messages in the folder
                    ImapMessageInfoCollection messagesInfo = imapClient.ListMessages();

                    // Log size of each fetched message in kilobytes
                    foreach (ImapMessageInfo messageInfo in messagesInfo)
                    {
                        // Fetch the full message (ensures proper disposal)
                        using (MailMessage message = imapClient.FetchMessage(messageInfo.UniqueId))
                        {
                            long sizeBytes = messageInfo.Size; // Size from MessageInfoBase
                            double sizeKilobytes = sizeBytes / 1024.0;
                            Console.WriteLine($"Message UID {messageInfo.UniqueId}: {sizeKilobytes:F2} KB");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
