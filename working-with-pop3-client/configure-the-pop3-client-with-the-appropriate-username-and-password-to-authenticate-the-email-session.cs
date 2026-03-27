using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server configuration
                string host = "pop.example.com";
                string username = "user@example.com";
                string password = "password";

                // Initialize and authenticate the POP3 client
                try
                {
                    using (Pop3Client client = new Pop3Client(host, username, password))
                    {
                        // Validate the provided credentials
                        client.ValidateCredentials();
                        Console.WriteLine("POP3 client authenticated successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 client error: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
