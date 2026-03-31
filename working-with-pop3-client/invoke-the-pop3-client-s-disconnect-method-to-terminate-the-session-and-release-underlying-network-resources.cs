using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when using placeholder credentials/host
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 host detected. Skipping connection.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 client connected successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection error: {ex.Message}");
                    return;
                }

                // Perform any required operations here (e.g., list messages)

                // Explicitly disconnect the client to release network resources
                client.Dispose(); // Disconnect
                Console.WriteLine("POP3 client disconnected.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
