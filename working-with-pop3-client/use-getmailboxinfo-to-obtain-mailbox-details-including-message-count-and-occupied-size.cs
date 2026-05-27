using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Retrieve mailbox information
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Output the details
                    Console.WriteLine($"Message Count: {mailboxInfo.MessageCount}");
                    Console.WriteLine($"Occupied Size (bytes): {mailboxInfo.OccupiedSize}");
                }
                catch (Exception ex)
                {
                    // Handle client-specific errors gracefully
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
