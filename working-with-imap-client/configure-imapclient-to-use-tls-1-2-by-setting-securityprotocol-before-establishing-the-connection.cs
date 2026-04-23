using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Enforce TLS 1.2 for the connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create and configure the ImapClient
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.SSLAuto))
            {
                try
                {
                    // Perform a lightweight operation to validate the connection
                    client.SelectFolder("INBOX");
                    Console.WriteLine("Successfully connected to IMAP server using TLS 1.2 and selected INBOX.");
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
