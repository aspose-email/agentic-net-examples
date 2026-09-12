using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

// Author: Aspose.Email example
class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters
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

            // Create the IMAP client with automatic security negotiation
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Attempt a simple operation to verify authentication (select INBOX)
                    imapClient.SelectFolder("INBOX");
                    Console.WriteLine("IMAP authentication succeeded.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP authentication failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
