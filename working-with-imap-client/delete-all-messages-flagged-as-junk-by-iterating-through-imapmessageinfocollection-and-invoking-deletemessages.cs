using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Attempt to access the Junk folder.
                ImapFolderInfo junkFolder = client.MailboxInfo.JunkMessages;
                if (junkFolder == null)
                {
                    Console.Error.WriteLine("Junk folder not available on this server.");
                    return;
                }

                // Select the Junk folder.
                client.SelectFolder(junkFolder.Name);

                // Retrieve all messages in the Junk folder.
                IEnumerable<ImapMessageInfo> junkMessages = client.ListMessages();

                // Delete the messages if any are present.
                List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>(junkMessages);
                if (messagesToDelete.Count > 0)
                {
                    client.DeleteMessages(messagesToDelete, true); // commitNow = true
                    Console.WriteLine($"{messagesToDelete.Count} junk messages deleted.");
                }
                else
                {
                    Console.WriteLine("No junk messages found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
