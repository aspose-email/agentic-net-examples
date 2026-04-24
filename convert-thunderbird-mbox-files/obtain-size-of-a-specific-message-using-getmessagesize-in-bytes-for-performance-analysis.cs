using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server connection details (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls in CI
            if (host.Contains("example.com") || username.Contains("username") || password.Contains("password"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operation.");
                return;
            }

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate POP3 credentials: {ex.Message}");
                    return;
                }

                // Index of the message whose size we want (1‑based)
                int messageIndex = 1;

                // Retrieve the size of the specified message in bytes
                long messageSize = client.GetMessageSize(messageIndex);
                Console.WriteLine($"Message #{messageIndex} size: {messageSize} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
