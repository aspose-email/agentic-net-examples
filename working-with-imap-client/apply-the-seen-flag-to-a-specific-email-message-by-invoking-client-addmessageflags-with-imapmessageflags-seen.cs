using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Tools.Search;

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

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Retrieve the list of messages in the folder
                ImapMessageInfoCollection messages = client.ListMessages();

                // Ensure there is at least one message to operate on
                if (messages != null && messages.Count > 0)
                {
                    // Get the unique identifier of the first message
                    string uid = messages[0].UniqueId;

                    // Apply the Seen (IsRead) flag to the message
                    client.AddMessageFlags(uid, ImapMessageFlags.IsRead);

                    Console.WriteLine("Seen flag applied to message UID: " + uid);
                }
                else
                {
                    Console.WriteLine("No messages found in the INBOX folder.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
