using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize and connect the IMAP client
            using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
            {
                // Select the mailbox folder (e.g., INBOX)
                client.SelectFolder("INBOX");

                // Retrieve the list of messages in the selected folder
                ImapMessageInfoCollection messages = client.ListMessages();

                if (messages != null && messages.Count > 0)
                {
                    // Get the unique identifier of the first message
                    string messageId = messages[0].UniqueId;

                    // Delete the message and commit the deletion
                    client.DeleteMessage(messageId, true);
                    Console.WriteLine("Message with UID '{0}' has been deleted.", messageId);
                }
                else
                {
                    Console.WriteLine("No messages found to delete.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
