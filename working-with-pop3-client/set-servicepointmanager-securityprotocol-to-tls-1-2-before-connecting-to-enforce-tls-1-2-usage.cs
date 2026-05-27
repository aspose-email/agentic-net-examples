using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Enforce TLS 1.2 for all connections
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string host = "pop3.example.com";
            int port = 110;
            string username = "user";
            string password = "pass";

            // Skip real network call when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping POP3 connection.");
                return;
            }

            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 credentials validated successfully.");
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
