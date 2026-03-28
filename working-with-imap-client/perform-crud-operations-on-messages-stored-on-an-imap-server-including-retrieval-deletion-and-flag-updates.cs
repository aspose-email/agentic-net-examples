using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Retrieve the list of messages in the selected folder
                ImapMessageInfoCollection messages = client.ListMessages();

                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                }

                if (messages.Count > 0)
                {
                    // Work with the first message in the collection
                    ImapMessageInfo firstMessage = messages[0];

                    // Fetch the full message content
                    MailMessage fullMessage = client.FetchMessage(firstMessage.UniqueId);
                    Console.WriteLine($"Fetched message body: {fullMessage.Body}");

                    // Mark the message as read (Seen flag)
                    client.AddMessageFlags(firstMessage.UniqueId, ImapMessageFlags.IsRead);

                    // Remove the read flag
                    client.RemoveMessageFlags(firstMessage.UniqueId, ImapMessageFlags.IsRead);

                    // Delete the message and commit the deletion
                    client.DeleteMessage(firstMessage.UniqueId, true);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
