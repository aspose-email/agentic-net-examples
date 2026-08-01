using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

// Author: Example code for creating a folder using Aspose.Email IMAP client
class Program
{
    static void Main()
    {
        try
        {
            // Connection settings (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize IMAP client
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Port = port;
                client.SecurityOptions = SecurityOptions.SSLImplicit;
                client.Username = username;
                client.Password = password;

                try
                {
                    // Create a new folder named "NewFolder" in the mailbox root
                    client.CreateFolder("NewFolder");
                    Console.WriteLine("Folder 'NewFolder' created successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
