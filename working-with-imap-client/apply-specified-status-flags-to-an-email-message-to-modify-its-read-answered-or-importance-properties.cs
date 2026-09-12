using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    public static void Main(string[] args)
    {
        // IMAP server connection settings
        string host = "imap.example.com";
        int port = 993;
        string username = "user@example.com";
        string password = "password";
        string folderName = "INBOX";

        // Skip external calls when placeholder credentials are used
        if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        string messageUid = "12345"; // UID of the message to modify

        try
        {
            // Initialize and configure the IMAP client
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Select the target folder
                client.SelectFolder(folderName);

                // Add the "Seen" (read) flag
                client.AddMessageFlags(messageUid, ImapMessageFlags.IsRead);

                // Add the "Answered" flag
                client.AddMessageFlags(messageUid, ImapMessageFlags.Answered);

                Console.WriteLine("Message flags updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
