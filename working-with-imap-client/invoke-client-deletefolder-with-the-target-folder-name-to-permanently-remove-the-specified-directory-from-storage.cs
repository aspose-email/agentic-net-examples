using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

// Author: Aspose.Email example for deleting a folder via ImapClient
class Program
{
    static void Main()
    {
        try
        {
            // Server connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "TestFolder";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the ImapClient
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Delete the specified folder
                try
                {
                    client.DeleteFolder(folderName);
                    Console.WriteLine($"Folder '{folderName}' deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
