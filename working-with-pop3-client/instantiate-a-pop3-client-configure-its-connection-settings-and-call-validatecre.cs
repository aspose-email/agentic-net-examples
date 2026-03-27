using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize POP3 client with connection settings
            using (Pop3Client client = new Pop3Client("pop.example.com", 110, "username", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Validate the credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
