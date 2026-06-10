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
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operation.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate credentials before proceeding
                    client.ValidateCredentials();

                    // Retrieve the total number of messages in the mailbox
                    int totalMessages = client.GetMessageCount();

                    // Index of the message we intend to delete (1‑based)
                    int messageIndex = 5;

                    // Validate the index to avoid ArgumentException
                    if (messageIndex < 1 || messageIndex > totalMessages)
                    {
                        Console.WriteLine($"Invalid message index {messageIndex}. Mailbox contains {totalMessages} messages.");
                    }
                    else
                    {
                        // Delete the specified message and commit the changes
                        client.DeleteMessage(messageIndex);
                        Console.WriteLine($"Message at index {messageIndex} has been deleted.");
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
