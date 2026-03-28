using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapPrerequisiteCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with real values as needed)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                SecurityOptions security = SecurityOptions.Auto;

                // Verify that all required parameters are provided
                if (string.IsNullOrWhiteSpace(host))
                {
                    Console.Error.WriteLine("IMAP host is not specified.");
                    return;
                }

                if (port <= 0 || port > 65535)
                {
                    Console.Error.WriteLine("IMAP port is invalid.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.Error.WriteLine("IMAP username is not specified.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("IMAP password is not specified.");
                    return;
                }

                // Create and use the IMAP client inside a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, port, username, password, security))
                {
                    try
                    {
                        // Attempt to select the INBOX folder to verify connection and credentials
                        client.SelectFolder("INBOX");
                        Console.WriteLine("IMAP connection and credentials are valid. INBOX folder selected successfully.");
                    }
                    catch (Exception connectionEx)
                    {
                        Console.Error.WriteLine($"Failed to connect or authenticate to IMAP server: {connectionEx.Message}");
                        // No rethrow; exit gracefully
                    }
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
