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
            // Connection parameters
            string host = "pop3.example.com";
            int port = 995; // Standard POP3 over SSL port
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Security mode (SSL/TLS implicit)
            SecurityOptions security = SecurityOptions.SSLImplicit;

            // Initialize POP3 client with explicit settings
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // The client connects automatically on first operation.
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");
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
