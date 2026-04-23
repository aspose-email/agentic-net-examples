using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Create and use the ImapClient instance
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "IMAP connection successful." : "IMAP credentials validation failed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
