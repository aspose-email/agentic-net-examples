using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with host, username, and password
            using (Pop3Client client = new Pop3Client("pop.example.com", "username", "password"))
            {
                try
                {
                    // Validate the credentials against the POP3 server
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
