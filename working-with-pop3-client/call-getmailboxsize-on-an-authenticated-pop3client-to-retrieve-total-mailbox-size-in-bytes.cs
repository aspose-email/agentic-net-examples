using Aspose.Email.Clients;
using System;
using Aspose.Email.Clients.Pop3;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping mailbox size retrieval.");
                return;
            }

            // Create and authenticate POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve total mailbox size in bytes
                    long mailboxSize = client.GetMailboxSize();
                    Console.WriteLine($"Mailbox size: {mailboxSize} bytes");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving mailbox size: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
