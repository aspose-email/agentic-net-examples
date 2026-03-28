using System;
using System.Collections.Generic;
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
                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Retrieve messages from the selected folder
                ImapMessageInfoCollection messages = client.ListMessages();

                // Find a message that has the Flagged flag set
                foreach (ImapMessageInfo info in messages)
                {
                    if (info.Flagged)
                    {
                        // Remove the Flagged flag from the message
                        client.RemoveMessageFlags(new[] { info }, ImapMessageFlags.Flagged);
                        Console.WriteLine($"Removed Flagged flag from message UID {info.UniqueId}");
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
