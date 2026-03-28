using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection details
            string host = "pop.example.com";
            int port = 110; // default POP3 port
            string username = "user@example.com";
            string password = "password";

            // Create and configure the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate the provided credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
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
