using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server details (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip actual connection when placeholder values are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping connection.");
                return;
            }

            // Instantiate the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();
                    Console.WriteLine("Connected and authenticated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
