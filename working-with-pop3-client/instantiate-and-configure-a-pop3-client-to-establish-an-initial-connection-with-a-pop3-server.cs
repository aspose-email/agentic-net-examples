using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with server details
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = "pop.example.com";
                client.Port = 110;
                client.Username = "user@example.com";
                client.Password = "password";
                client.SecurityOptions = SecurityOptions.Auto;

                try
                {
                    // Validate credentials to establish connection
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 client connected successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect POP3 client: {ex.Message}");
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
