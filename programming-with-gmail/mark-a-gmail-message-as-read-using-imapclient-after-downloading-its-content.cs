using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network call if they are not replaced.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            if (host.Contains("example.com") || username.Contains("example"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            // Path to save the downloaded message.
            string downloadPath = "downloaded.eml";

            // Ensure the directory for the download path exists.
            string downloadDirectory = Path.GetDirectoryName(Path.GetFullPath(downloadPath));
            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            // Use the ImapClient inside a using block to guarantee disposal.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Select the INBOX folder.
                client.SelectFolder("INBOX");

                // Retrieve the list of messages in the INBOX.
                ImapMessageInfoCollection messages = client.ListMessages();

                // If there are no messages, exit gracefully.
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in INBOX.");
                    return;
                }

                // Take the first message as an example.
                ImapMessageInfo messageInfo = messages[0];
                string uniqueId = messageInfo.UniqueId;

                // Download the message content.
                MailMessage mailMessage = client.FetchMessage(uniqueId);

                // Save the message to a local file.
                try
                {
                    mailMessage.Save(downloadPath);
                    Console.WriteLine($"Message saved to {downloadPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    return;
                }

                // Mark the message as read by adding the Read flag.
                client.AddMessageFlags(uniqueId, ImapMessageFlags.IsRead);
                Console.WriteLine("Message marked as read.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
