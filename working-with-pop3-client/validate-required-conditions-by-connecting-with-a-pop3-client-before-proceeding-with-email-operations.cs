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
            // POP3 server configuration
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize and use POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate credentials and establish connection
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 connection successful.");

                    // Retrieve message count
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Message count: {messageCount}");

                    // Example operation: fetch first message if any
                    if (messageCount > 0)
                    {
                        using (MailMessage message = client.FetchMessage(1))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
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
