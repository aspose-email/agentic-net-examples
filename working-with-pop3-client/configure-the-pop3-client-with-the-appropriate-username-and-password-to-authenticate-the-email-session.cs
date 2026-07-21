using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

// Author: Aspose.Email POP3 authentication example
class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize the POP3 client with host, username, and password
            using (Pop3Client pop3Client = new Pop3Client(host, username, password))
            {
                // Retrieve the total number of messages in the mailbox
                int messageCount = pop3Client.GetMessageCount();
                Console.WriteLine($"Number of messages: {messageCount}");

                // If there are messages, fetch and display the subject of the first one
                if (messageCount > 0)
                {
                    MailMessage firstMessage = pop3Client.FetchMessage(1);
                    Console.WriteLine($"Subject of first message: {firstMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
