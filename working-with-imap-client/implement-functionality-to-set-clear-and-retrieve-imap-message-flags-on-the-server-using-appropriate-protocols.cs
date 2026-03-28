using System;
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
            // Server connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, security))
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    if (messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Work with the first message
                    ImapMessageInfo firstMessage = messages[0];
                    string uid = firstMessage.UniqueId; // Unique identifier (string)
                    ImapMessageFlags currentFlags = firstMessage.Flags;

                    Console.WriteLine($"Message UID: {uid}");
                    Console.WriteLine($"Current Flags: {currentFlags}");

                    // Add the \Flagged flag to the message
                    client.AddMessageFlags(uid, ImapMessageFlags.Flagged);
                    Console.WriteLine("Added \\Flagged flag.");

                    // Retrieve flags again to verify
                    ImapMessageInfo updatedMessage = client.ListMessage(uid);
                    Console.WriteLine($"Flags after adding: {updatedMessage.Flags}");

                    // Remove the \Flagged flag from the message
                    client.RemoveMessageFlags(uid, ImapMessageFlags.Flagged);
                    Console.WriteLine("Removed \\Flagged flag.");

                    // Retrieve flags again to verify removal
                    ImapMessageInfo finalMessage = client.ListMessage(uid);
                    Console.WriteLine($"Flags after removal: {finalMessage.Flags}");
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
