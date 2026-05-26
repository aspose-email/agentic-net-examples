using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com") || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping validation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                try
                {
                    // Validate credentials
                    bool isValid = client.ValidateCredentials();

                    if (isValid)
                    {
                        Console.WriteLine("SMTP credentials are valid.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Authentication failed: Invalid SMTP credentials.");
                    }
                }
                catch (SmtpException ex)
                {
                    // Detailed error handling for SMTP-specific exceptions
                    Console.Error.WriteLine($"SMTP error ({ex.StatusCode}): {ex.Message}");
                }
                catch (Exception ex)
                {
                    // General error handling
                    Console.Error.WriteLine($"Unexpected error during credential validation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
