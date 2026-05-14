using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder POP3 credentials detected. Skipping connection.");
                return;
            }

            // Create POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve mailbox information
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Access MessageCount property
                    int messageCount = mailboxInfo.MessageCount;

                    Console.WriteLine($"Number of messages in mailbox: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing mailbox: {ex.Message}");
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
