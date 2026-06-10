using Aspose.Email.Clients;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Initialize POP3 client with SSL implicit security
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();
                    Console.WriteLine("Connected and authenticated successfully.");

                    // Example operation: get message count
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Message count: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error connecting to POP3 server: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
