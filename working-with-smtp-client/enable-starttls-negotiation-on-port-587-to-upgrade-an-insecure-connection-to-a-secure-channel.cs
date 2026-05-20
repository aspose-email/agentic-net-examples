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
            // Server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping connection.");
                return;
            }

            // Initialize the SMTP client with STARTTLS (SSLExplicit) on port 587
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.SSLExplicit))
            {
                try
                {
                    // Validate the credentials; any failure is caught and reported
                    client.ValidateCredentials();
                    Console.WriteLine("STARTTLS negotiation succeeded and credentials are valid.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP operation failed: {ex.Message}");
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
