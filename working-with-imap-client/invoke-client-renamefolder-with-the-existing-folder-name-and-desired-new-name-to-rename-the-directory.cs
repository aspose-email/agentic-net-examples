using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: sample demonstrates renaming an IMAP folder using Aspose.Email.
            // Connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.SSLImplicit; // secure connection

                // Folder names
                string existingFolder = "OldFolder";
                string newFolder = "NewFolder";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                try
                {
                    // Rename the folder
                    client.RenameFolder(existingFolder, newFolder);
                    Console.WriteLine($"Folder '{existingFolder}' renamed to '{newFolder}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to rename folder: {ex.Message}");
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
