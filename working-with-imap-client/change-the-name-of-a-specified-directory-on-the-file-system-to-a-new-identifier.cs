using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Configuration – replace with actual server details and credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Folder names
            string oldFolderName = "OldFolder";
            string newFolderName = "NewFolder";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.Host = host;
                imapClient.Port = port;
                imapClient.Username = username;
                imapClient.Password = password;
                imapClient.SecurityOptions = SecurityOptions.SSLImplicit; // Auto‑negotiation can also be used

                // Rename the specified folder on the IMAP server
                imapClient.RenameFolder(oldFolderName, newFolderName);
                Console.WriteLine($"Folder '{oldFolderName}' successfully renamed to '{newFolderName}'.");
            }
        }
        catch (Exception ex)
        {
            // Graceful error handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
