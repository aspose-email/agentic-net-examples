using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string host = "pop3.example.com";
                int port = 110;
                string username = "username";
                string password = "password";

                // Guard to avoid real network call with placeholder credentials
                if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                    return;
                }

                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        client.ValidateCredentials();

                        int messageCount = client.GetMessageCount();

                        // Expected count – adjust as needed for the test environment
                        int expectedCount = 0;

                        if (messageCount == expectedCount)
                        {
                            Console.WriteLine($"Test passed. Message count: {messageCount}");
                        }
                        else
                        {
                            Console.Error.WriteLine($"Test failed. Expected {expectedCount} but got {messageCount}.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
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
