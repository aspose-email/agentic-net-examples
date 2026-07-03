using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server and credentials (placeholders)
            string host = "pop3.example.com";
            string username = "user";
            string password = "pass";

            // Proxy settings
            string proxyHost = "proxy.example.com";
            int proxyPort = 8080;

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Create POP3 client and configure proxy
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                client.Proxy = new HttpProxy(proxyHost, proxyPort);

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
