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
            // Author note: sample demonstrates establishing a POP3 connection with Aspose.Email.
            string host = "pop3.example.com";
            int port = 995; // SSL/TLS port
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create POP3 client with automatic security negotiation.
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // The client connects automatically on the first operation.
                int messageCount = pop3Client.GetMessageCount();
                Console.WriteLine($"Total messages: {messageCount}");

                int fetchCount = Math.Min(5, messageCount);
                for (int i = 1; i <= fetchCount; i++)
                {
                    MailMessage message = pop3Client.FetchMessage(i);
                    Console.WriteLine($"Message {i}: {message.Subject}");
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
