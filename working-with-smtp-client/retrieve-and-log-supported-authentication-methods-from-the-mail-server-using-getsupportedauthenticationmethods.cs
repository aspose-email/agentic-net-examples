using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping server connection.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Validate credentials to ensure a successful connection.
                    bool isValid = client.ValidateCredentials();
                    if (!isValid)
                    {
                        Console.WriteLine("Authentication failed. Check credentials.");
                        return;
                    }

                    // Retrieve supported authentication methods.
                    ImapKnownAuthenticationType supportedAuth = client.SupportedAuthentication;
                    Console.WriteLine($"Supported authentication methods: {supportedAuth}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Client operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
