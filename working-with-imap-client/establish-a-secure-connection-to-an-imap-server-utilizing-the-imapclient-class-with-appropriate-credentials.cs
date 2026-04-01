using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapSecureConnectionSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // IMAP server details (placeholders)
                string host = "imap.example.com";
                int port = 993; // Secure IMAP port
                string username = "user@example.com";
                string password = "password";

                // Skip actual connection when placeholder values are detected
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                    return;
                }

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Validate credentials and establish a secure connection
                        client.ValidateCredentials();
                        Console.WriteLine("Successfully connected to the IMAP server.");
                        
                        // Additional IMAP operations can be performed here
                    }
                    catch (Exception connectionEx)
                    {
                        Console.Error.WriteLine($"IMAP connection failed: {connectionEx.Message}");
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
}
