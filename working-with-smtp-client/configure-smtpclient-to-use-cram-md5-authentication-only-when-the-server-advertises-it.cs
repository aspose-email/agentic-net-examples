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
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("@example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping connection.");
                return;
            }

            // Create the SMTP client with automatic TLS selection
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.SSLAuto))
            {
                // Retrieve the authentication mechanisms advertised by the server
                SmtpKnownAuthenticationType supportedAuth = client.SupportedAuthentication;

                // Check if CRAM-MD5 is supported
                if ((supportedAuth & SmtpKnownAuthenticationType.CramMD5) != 0)
                {
                    // Restrict client to use only CRAM-MD5 authentication
                    client.AllowedAuthentication = SmtpKnownAuthenticationType.CramMD5;
                    Console.WriteLine("CRAM-MD5 authentication is supported and has been enabled.");
                }
                else
                {
                    Console.WriteLine("CRAM-MD5 authentication is not supported by the server.");
                }

                // Optional: validate credentials (will use the allowed authentication method)
                try
                {
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Credentials validation failed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation error: {ex.Message}");
                }

                // The client will be disposed automatically at the end of the using block
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
