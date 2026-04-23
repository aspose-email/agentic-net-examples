using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server details – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client.
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve messages from the selected folder.
                    ImapMessageInfoCollection messages = client.ListMessages();
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Prepare a collection with the first message to modify.
                    List<ImapMessageInfo> targetMessages = new List<ImapMessageInfo> { messages[0] };

                    // Mark the selected message(s) with the Deleted flag without expunging.
                    client.ChangeMessageFlags(targetMessages, ImapMessageFlags.Deleted);
                    Console.WriteLine($"Message UID {messages[0].UniqueId} marked as Deleted.");
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
