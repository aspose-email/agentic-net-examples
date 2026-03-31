using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapAuthCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder IMAP server credentials
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip real network call when placeholders are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Skipping IMAP authentication check due to placeholder credentials.");
                    return;
                }

                // Create and dispose the ImapClient
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Attempt to validate the credentials
                        bool isValid = client.ValidateCredentials();

                        if (isValid)
                        {
                            Console.WriteLine("IMAP authentication succeeded.");
                        }
                        else
                        {
                            Console.Error.WriteLine("IMAP authentication failed: Invalid credentials.");
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        // Handle IMAP-specific errors
                        Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        // Handle other possible errors (e.g., network issues)
                        Console.Error.WriteLine($"Error during authentication: {ex.Message}");
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
