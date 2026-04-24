using Aspose.Email;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network operation.");
                return;
            }

            // Create and dispose the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve messages from the folder
                    ImapMessageInfoCollection messages = client.ListMessages();
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Take the first message as an example
                    ImapMessageInfo firstMessage = messages[0];

                    // Define the flag to set (mark as read)
                    ImapMessageFlags flags = ImapMessageFlags.IsRead;

                    // Asynchronously change the message flags
                    Task changeTask = client.ChangeMessageFlagsAsync(firstMessage.UniqueId, flags, CancellationToken.None);
                    changeTask.Wait();

                    Console.WriteLine($"Message UID {firstMessage.UniqueId} flagged as read.");
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
