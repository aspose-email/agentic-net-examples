using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client with host, port, username, and password
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                // Set security options if needed
                client.SecurityOptions = SecurityOptions.Auto;

                // Validate credentials
                bool isAuthenticated = client.ValidateCredentials();
                Console.WriteLine(isAuthenticated ? "Authentication succeeded." : "Authentication failed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
