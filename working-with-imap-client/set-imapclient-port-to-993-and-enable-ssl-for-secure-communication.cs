using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual use
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Guard: skip network operations when placeholders are detected
            bool isPlaceholder = host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                                 username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                 password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Create and configure the ImapClient
            using (ImapClient client = new ImapClient())
            {
                client.Host = host;
                client.Username = username;
                client.Password = password;

                // Set secure port and enable SSL
                client.Port = 993;
                client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Optional: validate credentials safely
                try
                {
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine($"Credentials valid: {isValid}");
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
