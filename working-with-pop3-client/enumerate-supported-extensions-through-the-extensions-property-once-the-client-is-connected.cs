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
            // Placeholder credentials – skip actual network call in CI environments
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 host detected. Skipping connection.");
                return;
            }

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials to ensure connection is established
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                // Enumerate supported extensions (capabilities) using GetCapabilities()
                string[] extensions = client.GetCapabilities();
                Console.WriteLine("Supported POP3 extensions:");
                foreach (string ext in extensions)
                {
                    Console.WriteLine($"- {ext}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
