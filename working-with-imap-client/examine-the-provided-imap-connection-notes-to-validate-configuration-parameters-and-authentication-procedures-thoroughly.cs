using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients; // for SecurityOptions if needed

// Author: Aspose.Email .NET example author

namespace ImapConnectionValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configuration parameters (replace with real values or retrieve from a secure source)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "P@ssw0rd";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Basic validation of configuration values
                if (string.IsNullOrWhiteSpace(host))
                {
                    Console.Error.WriteLine("IMAP host is missing.");
                    return;
                }

                if (port <= 0 || port > 65535)
                {
                    Console.Error.WriteLine("IMAP port must be between 1 and 65535.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.Error.WriteLine("IMAP username is missing.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("IMAP password is missing.");
                    return;
                }

                // Create the ImapClient instance using the validated parameters
                using (ImapClient imapClient = new ImapClient(host, port, username, password))
                {
                    try
                    {
                        // Attempt to select the INBOX folder to verify authentication and connectivity
                        imapClient.SelectFolder("INBOX");
                        Console.WriteLine("IMAP connection and authentication succeeded.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect or authenticate to IMAP server: {ex.Message}");
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
