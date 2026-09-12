using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration
            string host = "pop3.example.com";
            int port = 110; // default POP3 port
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Instantiate the POP3 client (preserve variable name)
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
            {
                // Example operation: get total message count
                try
                {
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving message count: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
