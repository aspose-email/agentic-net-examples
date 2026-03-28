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
            // Initialize the IMAP client (replace with actual server details)
            using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
            {
                // Retrieve the list of messages in the default folder
                ImapMessageInfoCollection messages = client.ListMessages();
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the mailbox.");
                    return;
                }

                // Select the first message to modify its status flags
                ImapMessageInfo firstMessage = messages[0];

                // Combine the desired flags (e.g., mark as read and answered)
                ImapMessageFlags flagsToAdd = ImapMessageFlags.IsRead | ImapMessageFlags.Answered;

                // Apply the flags using the message's unique identifier
                client.AddMessageFlags(firstMessage.UniqueId, flagsToAdd);

                Console.WriteLine($"Updated flags for message UID {firstMessage.UniqueId}.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
