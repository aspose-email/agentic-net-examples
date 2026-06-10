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
            // Define connection parameters (replace with real values for actual use)
            string host = "pop3.example.com";
            int port = 995; // non‑standard POP3 port
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholder values are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();

                    // Example operation: list messages count
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Number of messages on server: {messageCount}");
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
