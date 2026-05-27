using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network call in CI environments
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder POP3 settings detected. Skipping connection attempt.");
                return;
            }

            // Measure round‑trip time of the connection (ValidateCredentials triggers connection)
            DateTime startTime = DateTime.UtcNow;

            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Attempt to validate credentials which forces a connection to the server
                    bool isValid = client.ValidateCredentials();
                    DateTime endTime = DateTime.UtcNow;
                    TimeSpan duration = endTime - startTime;

                    Console.WriteLine($"Connection validation result: {isValid}");
                    Console.WriteLine($"Round‑trip time: {duration.TotalMilliseconds} ms");
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"POP3 error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
