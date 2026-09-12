using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ValidateImapCredentials
{
    // Author: Aspose.Email example
    class Program
    {
        static void Main()
        {
            // Define connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials
            if (host.Contains("example", StringComparison.OrdinalIgnoreCase) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Placeholder credentials detected – skipping validation.");
                return;
            }

            try
            {
                // Create the IMAP client with SSL implicit security
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Validate the supplied credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
            }
        }
    }
}
