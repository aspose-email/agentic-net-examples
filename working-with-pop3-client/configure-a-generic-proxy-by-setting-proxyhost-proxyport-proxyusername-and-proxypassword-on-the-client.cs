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
            // POP3 server details (replace with real values when needed)
            string pop3Host = "pop3.example.com";
            int pop3Port = 110;
            string username = "user@example.com";
            string password = "password";

            // Proxy configuration
            string proxyHost = "proxy.example.com";
            int proxyPort = 8080;
            string proxyUsername = "proxyUser";
            string proxyPassword = "proxyPass";

            // Skip actual connection when using placeholder data
            if (pop3Host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder server details detected. Proxy configuration demonstrated without connecting.");
                return;
            }

            using (Pop3Client client = new Pop3Client(pop3Host, pop3Port, username, password))
            {
                // Assign a generic HTTP proxy to the client
                client.Proxy = new HttpProxy(proxyHost, proxyPort, proxyUsername, proxyPassword);

                // Optional: validate credentials to ensure the client works with the proxy
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("Proxy configured and credentials validated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
