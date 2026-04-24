using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapCredentialValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // IMAP server settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Skip validation if placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder IMAP settings detected. Skipping credential validation.");
                    return;
                }

                // Initialize IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        bool credentialsValid = client.ValidateCredentials();
                        if (credentialsValid)
                        {
                            Console.WriteLine("IMAP credentials are valid.");
                        }
                        else
                        {
                            Console.Error.WriteLine("IMAP credentials are invalid.");
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP validation error: {imapEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during validation: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
