using Aspose.Email;
using System;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailSmtpValidate
{
    class Program
    {
        static void Main()
        {
            try
            {
                // SMTP server configuration
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Skip validation when placeholder credentials are detected
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping validation.");
                    return;
                }

                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    try
                    {
                        // Validate the credentials without sending an email
                        bool isValid = client.ValidateCredentials();

                        Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during validation
                        Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
