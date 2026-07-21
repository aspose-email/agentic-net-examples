using Aspose.Email.Clients;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with server credentials
            Pop3Client pop3Client = new Pop3Client
            {
                Host = "pop3.example.com",
                Port = 110,
                Username = "user@example.com",
                Password = "password"
                // Uncomment and adjust if SSL/TLS is required
                // SecurityOptions = SecurityOptions.Auto
            };

            // Guard to avoid real network calls when placeholders are used
            bool isPlaceholder = pop3Client.Host.Contains("example.com") ||
                                 pop3Client.Username.Contains("example.com") ||
                                 pop3Client.Password == "password";

            if (isPlaceholder)
            {
                Console.WriteLine("Skipping POP3 operations due to placeholder credentials.");
            }
            else
            {
                // Perform a simple operation to establish the connection
                int messageCount = pop3Client.GetMessageCount();
                Console.WriteLine($"Message count: {messageCount}");

                // Explicitly release network resources
                pop3Client.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
