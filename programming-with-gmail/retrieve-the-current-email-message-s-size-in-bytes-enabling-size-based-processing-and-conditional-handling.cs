using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder POP3 server credentials
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com") || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operation.");
                return;
            }

            // Initialize POP3 client (connection is established via constructor)
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Validate credentials before proceeding
                    client.ValidateCredentials();

                    // Get total number of messages in the mailbox
                    int messageCount = client.GetMessageCount();

                    // Iterate through each message and retrieve its size
                    for (int i = 1; i <= messageCount; i++)
                    {
                        long sizeInBytes = client.GetMessageInfo(i).Size;
                        Console.WriteLine($"Message {i} size: {sizeInBytes} bytes");
                    }
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
