using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3TimeoutExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection details (placeholders)
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Skip real network call when placeholders are detected
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder POP3 host detected. Skipping connection.");
                    return;
                }

                // Create POP3 client with explicit timeout of 15 seconds (15000 ms)
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    client.Timeout = 15000; // 15,000 milliseconds

                    try
                    {
                        // Validate credentials to ensure connection works
                        client.ValidateCredentials();

                        // Example operation: retrieve message count
                        int messageCount = client.GetMessageCount();
                        Console.WriteLine($"Message count: {messageCount}");
                    }
                    catch (Exception operationEx)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {operationEx.Message}");
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
