using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials check – skip actual network call in CI environments.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Connect securely using SSL/TLS (implicit) after authentication.
            try
            {
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.SSLImplicit))
                {
                    // Perform a lightweight operation to confirm the connection.
                    client.SelectFolder("INBOX");
                    Console.WriteLine("Successfully connected to IMAP server with SSL/TLS.");
                }
            }
            catch (ImapException imapEx)
            {
                Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
