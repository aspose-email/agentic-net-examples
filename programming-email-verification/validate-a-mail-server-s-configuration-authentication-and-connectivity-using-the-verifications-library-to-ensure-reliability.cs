using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace EmailServerVerification
{
    class Program
    {
        static void Main()
        {
            // Server configuration – replace with real values or obtain from configuration.
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Wrap the client in a using block to ensure proper disposal.
            try
            {
                using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Attempt to validate the credentials against the SMTP server.
                    bool isValid = client.ValidateCredentials();

                    if (isValid)
                    {
                        Console.WriteLine("SMTP server credentials are valid. Connectivity test succeeded.");
                    }
                    else
                    {
                        Console.WriteLine("SMTP server credentials are invalid or the server rejected the connection.");
                    }
                }
            }
            catch (SmtpException ex)
            {
                // Handles SMTP-specific errors (e.g., authentication failures, connection issues).
                Console.Error.WriteLine($"SMTP error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handles any other unexpected errors.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
