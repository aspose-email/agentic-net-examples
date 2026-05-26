using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string host = "smtp.example.com";
            int port = 25;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP server detected. Skipping connection.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    // Retrieve server capabilities (extensions)
                    IEnumerable<string> capabilities = client.GetCapabilities();

                    // Check for STARTTLS support
                    bool startTlsSupported = capabilities != null &&
                                             capabilities.Any(c => c.Equals("STARTTLS", StringComparison.OrdinalIgnoreCase));

                    if (startTlsSupported)
                    {
                        // Enable explicit TLS (STARTTLS)
                        client.SecurityOptions = SecurityOptions.SSLExplicit;
                        Console.WriteLine("STARTTLS extension found. Enabled SSLExplicit.");
                    }
                    else
                    {
                        Console.WriteLine("STARTTLS extension not found. Using default security options.");
                    }

                    // Optional: validate credentials after setting security options
                    bool authOk = client.ValidateCredentials();
                    Console.WriteLine($"Credentials validation result: {authOk}");
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
