using System;
using Aspose.Email;
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
                // Initialize POP3 client with host, username, password and security options
                using (Pop3Client client = new Pop3Client("pop.example.com", "username", "password", SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate credentials to ensure connection is successful
                        client.ValidateCredentials();
                        Console.WriteLine("POP3 client connected and authenticated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 connection failed: {ex.Message}");
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
}
