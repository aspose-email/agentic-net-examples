using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Connection parameters
                string host = "pop.example.com";
                int port = 110; // Default POP3 port
                string username = "user@example.com";
                string password = "password";

                // Instantiate POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        // Validate credentials and establish connection
                        client.ValidateCredentials();
                        Console.WriteLine("Connected to POP3 server successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect: {ex.Message}");
                        return;
                    }

                    // Additional POP3 operations can be performed here
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
