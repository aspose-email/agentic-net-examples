using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Instantiate the POP3 client with host, username, and password.
            using (Pop3Client client = new Pop3Client("pop.example.com", "username", "password"))
            {
                try
                {
                    // Validate the credentials to ensure the client can connect.
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 client initialized and credentials validated.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
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
