using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Instantiate POP3 client with host, port, username, password, and security options
            using (Pop3Client client = new Pop3Client("pop.example.com", 110, "username", "password", SecurityOptions.Auto))
            {
                // Validate credentials to ensure connection parameters are correct
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 client created and credentials validated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
