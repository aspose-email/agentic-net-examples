using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapClientSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder values are detected.
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                    return;
                }

                // Instantiate ImapClient with SSL enabled.
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Validate the credentials.
                        bool isAuthenticated = client.ValidateCredentials();
                        Console.WriteLine(isAuthenticated ? "Authentication succeeded." : "Authentication failed.");
                    }
                    catch (Exception authEx)
                    {
                        Console.Error.WriteLine($"Authentication error: {authEx.Message}");
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
