using Aspose.Email;
using System;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients.Base;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (placeholders)
            string host = "smtp.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip network operations when placeholders are used
            bool isPlaceholder = host.Contains("example.com") ||
                                 username.Contains("example.com") ||
                                 password == "password";

            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping SMTP client configuration.");
                return;
            }

            // Create the SMTP client
            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                try
                {
                    // Configure the client to use TLS 1.2 only
                    client.SupportedEncryption = EncryptionProtocols.Tls12;

                    Console.WriteLine("SMTP client configured to use TLS 1.2.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error configuring client: {ex.Message}");
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
