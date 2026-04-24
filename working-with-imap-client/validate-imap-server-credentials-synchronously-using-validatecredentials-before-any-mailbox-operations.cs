using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server details
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Skipping IMAP credential validation due to placeholder values.");
                return;
            }

            // Create the IMAP client and validate credentials
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    bool isValid = client.ValidateCredentials();
                    if (isValid)
                    {
                        Console.WriteLine("IMAP credentials are valid.");
                    }
                    else
                    {
                        Console.WriteLine("IMAP credentials are invalid.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
