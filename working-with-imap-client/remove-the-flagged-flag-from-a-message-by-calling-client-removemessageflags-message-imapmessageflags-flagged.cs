using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

// Author: Aspose.Email example - remove Flagged flag from a message
class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection settings (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the ImapClient
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve messages from the selected folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    if (messages != null && messages.Count > 0)
                    {
                        // Get the unique identifier of the first message
                        ImapMessageInfo firstMessage = messages[0];
                        string uniqueId = firstMessage.UniqueId;

                        // Remove the Flagged flag from the message
                        client.RemoveMessageFlags(uniqueId, ImapMessageFlags.Flagged);
                        Console.WriteLine("Flagged flag removed from message UID: " + uniqueId);
                    }
                    else
                    {
                        Console.WriteLine("No messages found in INBOX.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("IMAP operation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
