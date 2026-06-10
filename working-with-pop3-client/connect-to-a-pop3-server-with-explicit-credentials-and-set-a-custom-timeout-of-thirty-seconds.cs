using System;
using Aspose.Email;
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

            // Skip actual connection when placeholder credentials are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Create and configure the POP3 client
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.Timeout = 30000; // 30 seconds

                try
                {
                    // Validate the credentials and establish the connection
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 connection established and credentials validated.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 connection error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
