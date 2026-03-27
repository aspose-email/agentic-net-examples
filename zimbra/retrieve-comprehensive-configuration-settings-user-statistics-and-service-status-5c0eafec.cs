using System.Collections.Generic;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            // Server connection parameters (replace with actual values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Initialize IMAP client with SSL implicit security
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Validate credentials (connection safety)
                try
                {
                    imapClient.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Authentication failed: " + ex.Message);
                    return;
                }

                // Display basic configuration settings
                Console.WriteLine("IMAP Server: " + imapClient.Host);
                Console.WriteLine("Port: " + imapClient.Port);
                Console.WriteLine("Security: " + imapClient.SecurityOptions);
                Console.WriteLine("Connection State: " + imapClient.ConnectionState);

                // Retrieve messages from INBOX folder
                ImapMessageInfoCollection inboxMessages = imapClient.ListMessages("INBOX");
                Console.WriteLine("Total messages in INBOX: " + inboxMessages.Count);

                // Example of retrieving a few message subjects for user statistics
                int previewCount = Math.Min(5, inboxMessages.Count);
                for (int i = 0; i < previewCount; i++)
                {
                    ImapMessageInfo messageInfo = inboxMessages[i];
                    Console.WriteLine("Message UID: " + messageInfo.UniqueId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}