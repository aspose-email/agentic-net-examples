using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details
                string host = "pop3.example.com";
                int port = 110;
                string username = "username";
                string password = "password";

                // Skip real connection when placeholders are used
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                    return;
                }

                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        // Retrieve mailbox information
                        Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                        long occupiedSize = mailboxInfo.OccupiedSize;
                        Console.WriteLine($"Mailbox occupied size: {occupiedSize} bytes");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving mailbox info: {ex.Message}");
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
}
