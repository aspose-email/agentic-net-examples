using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();
                    Console.WriteLine("POP3 client connected and authenticated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate POP3 client: {ex.Message}");
                    return;
                }

                // Example: retrieve message count
                try
                {
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Number of messages in mailbox: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving message count: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
