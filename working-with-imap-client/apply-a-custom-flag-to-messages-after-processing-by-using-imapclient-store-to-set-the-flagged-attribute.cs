using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping operation.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    // Apply the Flagged attribute to each message
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        try
                        {
                            client.AddMessageFlags(messageInfo.UniqueId, ImapMessageFlags.Flagged);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to set Flagged flag for message UID {messageInfo.UniqueId}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
