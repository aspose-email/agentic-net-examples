using Aspose.Email;
using System;
using Aspose.Email.Clients;
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

            // Skip execution when placeholders are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
            {
                client.Username = username;
                client.Password = password;

                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication failed: {ex.Message}");
                    return;
                }

                // Select the source folder (Inbox)
                try
                {
                    client.SelectFolder("Inbox");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select Inbox: {ex.Message}");
                    return;
                }

                // Retrieve messages from Inbox
                ImapMessageInfoCollection inboxMessages;
                try
                {
                    inboxMessages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Copy each message to Archive and preserve its flags
                foreach (ImapMessageInfo messageInfo in inboxMessages)
                {
                    try
                    {
                        // Copy the message to the Archive folder using its UID
                        client.CopyMessage(messageInfo.UniqueId.ToString(), "Archive");

                        // Preserve the original flags on the copied message
                        client.AddMessageFlags(messageInfo.UniqueId.ToString(), messageInfo.Flags);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message UID {messageInfo.UniqueId}: {ex.Message}");
                        // Continue with next message
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
