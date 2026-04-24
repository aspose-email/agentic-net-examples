using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapMoveExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip execution in CI environments
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                    return;
                }

                // Create and use the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Ensure the Archive folder exists
                        if (!client.ExistFolder("Archive"))
                        {
                            client.CreateFolder("Archive");
                        }

                        // Retrieve all messages in INBOX
                        ImapMessageInfoCollection messages = client.ListMessages();

                        // Move each message to the Archive folder
                        foreach (ImapMessageInfo messageInfo in messages)
                        {
                            client.MoveMessage(messageInfo.SequenceNumber, "Archive");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
