using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials detection – skip real network call in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP authentication.");
                return;
            }

            // Create the IMAP client with the provided credentials
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Asynchronously validate the credentials before any mailbox operation
                    bool isAuthenticated = await client.ValidateCredentialsAsync();
                    Console.WriteLine(isAuthenticated ? "Authentication succeeded." : "Authentication failed.");
                }
                catch (Exception authEx)
                {
                    Console.Error.WriteLine($"IMAP authentication error: {authEx.Message}");
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
