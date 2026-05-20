using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define SMTP server settings (replace with real values as needed)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping connection.");
                return;
            }

            // Initialize the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    // Set connection (greeting) timeout to 10 seconds (10000 ms)
                    client.GreetingTimeout = 10000;

                    // Set overall operation (read) timeout to 30 seconds (30000 ms)
                    client.Timeout = 30000;

                    Console.WriteLine($"SMTP client configured: GreetingTimeout={client.GreetingTimeout} ms, Timeout={client.Timeout} ms");
                    
                    // Example: validate credentials (optional)
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP client error: {ex.Message}");
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
