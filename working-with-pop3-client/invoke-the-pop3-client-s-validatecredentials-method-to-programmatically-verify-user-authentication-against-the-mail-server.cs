using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients;

namespace Pop3ValidateCredentialsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // POP3 server connection details
                string host = "pop3.example.com";
                int port = 110; // default POP3 port; change if needed
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the POP3 client (will be disposed automatically)
                using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
                {
                    // Validate credentials safely
                    try
                    {
                        bool credentialsValid = pop3Client.ValidateCredentials();
                        Console.WriteLine($"Credentials valid: {credentialsValid}");
                    }
                    catch (Exception ex)
                    {
                        // Connection/authentication errors are reported here
                        Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Any unexpected errors are caught here
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
