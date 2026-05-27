using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using System;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operation.");
                return;
            }

            // Index of the message to delete (must be positive)
            int messageIndex = 5; // Example index

            if (messageIndex <= 0)
            {
                Console.Error.WriteLine("Message index must be greater than zero.");
                return;
            }

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.None))
            {
                try
                {
                    // Delete the specified message
                    client.DeleteMessage(messageIndex);

                    // Commit the deletions so the server removes the message
                    Console.WriteLine($"Message at index {messageIndex} deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
