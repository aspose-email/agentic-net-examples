using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping connection.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Increase greeting timeout to 10 seconds (10000 ms)
                client.GreetingTimeout = 10000;

                try
                {
                    // Validate credentials (this will attempt to connect using the configured timeout)
                    client.ValidateCredentials();
                    Console.WriteLine("Credentials validated successfully with a 10‑second greeting timeout.");
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
