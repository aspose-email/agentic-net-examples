using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the IMAP client (replace placeholders with real values)
            using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
            {
                // Retrieve the collection of messages from the default folder (INBOX)
                ImapMessageInfoCollection messages = client.ListMessages();

                // Iterate through the messages and output their unique identifiers
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"Message UID: {info.UniqueId}");
                }
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
