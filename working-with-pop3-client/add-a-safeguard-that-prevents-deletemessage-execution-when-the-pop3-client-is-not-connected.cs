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
            // POP3 server configuration (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls in CI
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 configuration detected. Skipping execution.");
                return;
            }

            // Create and dispose the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials; this also establishes a connection
                    bool authenticated = client.ValidateCredentials();
                    if (!authenticated)
                    {
                        Console.Error.WriteLine("Authentication failed. Cannot proceed with delete operation.");
                        return;
                    }

                    // Safeguard: only attempt DeleteMessage if the client is authenticated/connected
                    // (ValidateCredentials succeeded, so we consider the client ready)
                    client.DeleteMessage(1);
                    Console.WriteLine("Message with sequence number 1 deleted successfully.");
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
