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
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (string.IsNullOrEmpty(host) || host.Contains("example.com") ||
                string.IsNullOrEmpty(username) || username.Contains("example.com"))
            {
                Console.WriteLine("Skipping execution due to placeholder credentials.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    client.Port = port;
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the selected folder.
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        // Check if the message has been replied to (Answered flag).
                        if (messageInfo.Answered)
                        {
                            // Remove the follow‑up flag (Flagged) from the message.
                            client.RemoveMessageFlags(messageInfo.UniqueId.ToString(), ImapMessageFlags.Flagged);

                            // Log the action for audit purposes.
                            Console.WriteLine($"Removed follow‑up flag from message UID {messageInfo.UniqueId} (Subject: {messageInfo.Subject})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
