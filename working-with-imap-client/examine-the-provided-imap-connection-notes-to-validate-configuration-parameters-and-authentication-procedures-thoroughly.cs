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
            // IMAP configuration parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.SSLImplicit; // Adjust as needed

            // Basic validation of configuration values
            if (string.IsNullOrWhiteSpace(host))
            {
                Console.Error.WriteLine("IMAP host is not specified.");
                return;
            }
            if (port <= 0 || port > 65535)
            {
                Console.Error.WriteLine("Invalid IMAP port number.");
                return;
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.Error.WriteLine("Username is not specified.");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Password is not specified.");
                return;
            }

            // Create the IMAP client and attempt connection/validation
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                try
                {
                    // Validate credentials with the server
                    client.ValidateCredentials();
                    Console.WriteLine("IMAP connection and authentication succeeded.");

                    // Perform a simple NOOP to verify server responsiveness
                    client.Noop();
                    Console.WriteLine("Server responded to NOOP command.");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error during IMAP operations: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
