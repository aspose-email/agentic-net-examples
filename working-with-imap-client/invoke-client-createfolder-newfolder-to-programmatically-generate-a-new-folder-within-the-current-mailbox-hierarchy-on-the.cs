using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials and host
                string host = "imap.example.com";
                string username = "username";
                string password = "password";

                // Guard against placeholder values to avoid real network calls
                if (host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                    username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                    password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping folder creation.");
                    return;
                }

                // Create and connect the IMAP client
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Create a new folder named "NewFolder"
                        client.CreateFolder("NewFolder");
                        Console.WriteLine("Folder 'NewFolder' created successfully.");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
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
