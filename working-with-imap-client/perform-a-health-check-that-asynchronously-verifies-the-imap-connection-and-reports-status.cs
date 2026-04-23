using Aspose.Email.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapHealthCheck
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip real network call when placeholders are used
                if (host.Contains("example.com") || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Placeholder credentials detected; skipping IMAP health check.");
                    return;
                }

                // Create and use the IMAP client
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Asynchronously validate credentials
                        bool isValid = await client.ValidateCredentialsAsync(null, CancellationToken.None);
                        if (isValid)
                        {
                            Console.WriteLine("IMAP connection successful.");
                        }
                        else
                        {
                            Console.Error.WriteLine("IMAP connection failed: invalid credentials.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP connection error: {ex.Message}");
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
