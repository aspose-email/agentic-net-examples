using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Skip external call when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder POP3 settings detected. Skipping connection.");
                return;
            }

            // Initialize POP3 client with credentials
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                client.UseAuthentication = true;

                // Validate credentials
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 client authenticated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
