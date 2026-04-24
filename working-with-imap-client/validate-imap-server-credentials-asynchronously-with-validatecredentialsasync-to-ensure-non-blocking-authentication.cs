using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define IMAP server credentials (placeholders)
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping validation.");
                return;
            }

            // Create and use the ImapClient
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Asynchronously validate credentials (synchronously wait for result)
                    bool isValid = client.ValidateCredentialsAsync().GetAwaiter().GetResult();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
