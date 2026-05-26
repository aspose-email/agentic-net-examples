using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string host = "smtp.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when using placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP server detected. Skipping connection.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();

                    // Retrieve server extensions/capabilities
                    var capabilities = client.GetCapabilities();

                    // Store extensions for later checks
                    List<string> extensions = new List<string>();
                    foreach (var item in capabilities)
                    {
                        extensions.Add(item);
                    }

                    // Example usage: check if a specific extension is supported
                    string extensionToCheck = "STARTTLS";
                    bool isSupported = extensions.Contains(extensionToCheck);
                    Console.WriteLine($"{extensionToCheck} supported: {isSupported}");
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
