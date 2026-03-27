using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration
            string host = "pop.example.com";
            int port = 110; // default POP3 port
            string username = "user@example.com";
            string password = "password";

            // Initialize the POP3 client with the specified settings
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate the provided credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
