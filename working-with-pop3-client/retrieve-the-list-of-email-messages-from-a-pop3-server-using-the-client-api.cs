using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3RetrieveExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server connection details
                string host = "pop.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and use the POP3 client
                using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Get total number of messages in the mailbox
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");

                    // Iterate through each message and display its subject
                    for (int i = 1; i <= messageCount; i++)
                    {
                        using (MailMessage message = pop3Client.FetchMessage(i))
                        {
                            Console.WriteLine($"Message {i}: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Output any errors to the error stream
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
