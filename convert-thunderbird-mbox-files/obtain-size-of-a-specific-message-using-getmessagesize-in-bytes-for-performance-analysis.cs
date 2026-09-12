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
            // Author note: Example demonstrates obtaining the size of a specific POP3 message.
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            string uniqueId = "12345"; // Unique identifier of the message whose size is required

            // Create and configure the POP3 client
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Retrieve the size of the message in bytes
                    long messageSize = pop3Client.GetMessageSize(uniqueId);
                    Console.WriteLine($"Message size (bytes): {messageSize}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving message size: {ex.Message}");
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
