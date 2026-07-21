using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Adjust the connection parameters to match your IMAP server.
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

            // Create and configure the IMAP client.
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.SecurityOptions = SecurityOptions.SSLImplicit;
                client.Username = username;
                client.Password = password;

                // Select the INBOX folder (implicit connection is performed here).
                client.SelectFolder("INBOX");

                // Retrieve all messages in the selected folder.
                IList<ImapMessageInfo> messages = client.ListMessages();

                // Iterate through each message and clear all flag attributes.
                foreach (ImapMessageInfo msgInfo in messages)
                {
                    // Remove all flags by applying the Empty flag set.
                    client.RemoveMessageFlags(msgInfo.UniqueId, ImapMessageFlags.Empty);
                }

                Console.WriteLine($"Cleared flags on {messages.Count} messages.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
