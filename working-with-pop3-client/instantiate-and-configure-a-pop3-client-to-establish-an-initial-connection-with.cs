using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3ClientExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection parameters
                string host = "pop.example.com";
                int port = 110; // Standard POP3 port
                string username = "user@example.com";
                string password = "password";

                // Initialize the POP3 client with the specified parameters
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Perform a simple NOOP command to verify the connection
                        client.Noop();
                        Console.WriteLine("POP3 connection established successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
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
