using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            // Connect to the IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the first message in the folder
                    ImapMessageInfo messageInfo = client.ListMessages(1).FirstOrDefault();
                    if (messageInfo == null)
                    {
                        Console.Error.WriteLine("No messages found in the INBOX.");
                        return;
                    }

                    // Display current flags
                    Console.WriteLine($"Current flags for UID {messageInfo.UniqueId}: {messageInfo.Flags}");

                    // Add the Flagged flag to the message
                    client.AddMessageFlags(messageInfo.UniqueId, ImapMessageFlags.Flagged);
                    Console.WriteLine("Added Flagged flag.");

                    // Retrieve the message again to verify the flag was added
                    ImapMessageInfo updatedInfo = client.ListMessage(messageInfo.UniqueId);
                    Console.WriteLine($"Flags after adding: {updatedInfo.Flags}");

                    // Remove the Flagged flag from the message
                    client.RemoveMessageFlags(messageInfo.UniqueId, ImapMessageFlags.Flagged);
                    Console.WriteLine("Removed Flagged flag.");

                    // Retrieve the message again to verify the flag was removed
                    ImapMessageInfo finalInfo = client.ListMessage(messageInfo.UniqueId);
                    Console.WriteLine($"Final flags: {finalInfo.Flags}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
