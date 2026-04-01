using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3Example
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder POP3 server credentials
                string host = "pop3.example.com";
                string username = "username";
                string password = "password";

                // Guard: skip real network call when placeholders are used
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder POP3 credentials detected. Skipping connection.");
                    return;
                }

                // Instantiate the POP3 client
                using (Pop3Client client = new Pop3Client(host, username, password))
                {
                    try
                    {
                        // Retrieve mailbox information
                        int messageCount = client.GetMessageCount();
                        Console.WriteLine($"Total messages in mailbox: {messageCount}");
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
}
