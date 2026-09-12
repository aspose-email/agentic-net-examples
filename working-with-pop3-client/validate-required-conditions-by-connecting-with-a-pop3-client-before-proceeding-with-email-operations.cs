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
            // Author note: Demonstrates POP3 connection validation before further email processing.
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the POP3 client.
            using (Pop3Client pop3Client = new Pop3Client())
            {
                pop3Client.Host = host;
                pop3Client.Port = port;
                pop3Client.Username = username;
                pop3Client.Password = password;
                pop3Client.SecurityOptions = SecurityOptions.Auto; // Adjust as needed.

                try
                {
                    // Implicitly connects when retrieving message count.
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Connected successfully. Message count: {messageCount}");

                    // Example: fetch the first message if any exist.
                    if (messageCount > 0)
                    {
                        MailMessage message = pop3Client.FetchMessage(1);
                        Console.WriteLine($"Subject: {message.Subject}");
                        message.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
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
