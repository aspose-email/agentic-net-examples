using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3Sample
{
    class Program
    {
        static void Main()
        {
            // Placeholder credentials – replace with real values for actual use.
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip network operations when placeholders are detected.
            bool isPlaceholder = host.Contains("example.com") ||
                                 username.Contains("example.com") ||
                                 password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            try
            {
                // Instantiate the POP3 client.
                using (Pop3Client pop3Client = new Pop3Client(host, username, password))
                {
                    // Example operation: retrieve the number of messages in the mailbox.
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Number of messages on server: {messageCount}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
