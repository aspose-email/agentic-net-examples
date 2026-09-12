using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

public class Program
{
    public static void Main(string[] args)
    {
        // Placeholder connection parameters
        string host = "imap.example.com";
        int port = 993;
        string username = "user@example.com";
        string password = "password";

        // Guard to avoid real network calls when placeholders are used
        if (host.Contains("example.com") || username.Contains("example.com"))
        {
            Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
            return;
        }

        try
        {
            // Initialize IMAP client with SSL implicit security
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Get list of message identifiers
                ImapMessageInfoCollection messagesInfo = client.ListMessages();

                Console.WriteLine($"Found {messagesInfo.Count} messages in INBOX.");

                // Iterate through messages and display subject
                foreach (ImapMessageInfo info in messagesInfo)
                {
                    MailMessage message = client.FetchMessage(info.UniqueId);
                    Console.WriteLine($"Subject: {message.Subject}");
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving IMAP messages: {ex.Message}");
        }
    }
}
