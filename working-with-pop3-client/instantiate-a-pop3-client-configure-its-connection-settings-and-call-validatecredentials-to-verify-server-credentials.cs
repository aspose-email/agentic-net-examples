using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates POP3 client credential validation.
            string host = "pop.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            SecurityOptions security = SecurityOptions.Auto;

            // Initialize POP3 client (preserve variable name 'pop3Client')
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, security))
            {
                // Validate the credentials against the server
                bool isValid = pop3Client.ValidateCredentials();
                Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
