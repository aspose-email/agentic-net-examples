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
            // Placeholder credentials – skip real connection in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping connection.");
                return;
            }

            // Create and configure the ImapClient
            using (ImapClient client = new ImapClient())
            {
                try
                {
                    client.Host = host;
                    client.Username = username;
                    client.Password = password;

                    // Set the secure IMAP port (SSL/TLS) before any authentication occurs
                    client.Port = 993;
                    client.SecurityOptions = SecurityOptions.SSLImplicit;

                    // Perform a lightweight operation to trigger connection/authentication
                    client.SelectFolder("INBOX");
                    Console.WriteLine("Connected to IMAP server and selected INBOX successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP client error: {ex.Message}");
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
