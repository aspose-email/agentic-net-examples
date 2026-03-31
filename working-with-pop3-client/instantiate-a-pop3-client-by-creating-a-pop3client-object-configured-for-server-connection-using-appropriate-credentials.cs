using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials for demonstration
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Guard to avoid real network calls when placeholders are used
            if (host.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Instantiate and configure the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                // Example configuration (optional)
                client.SecurityOptions = SecurityOptions.Auto;

                // No actual network operation is performed in this sample
                Console.WriteLine($"POP3 client configured for host: {client.Host}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
