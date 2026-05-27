using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Skip actual connection when using placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Skipping POP3 connection due to placeholder credentials.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Retrieve all messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    Console.WriteLine($"Total messages: {messages.Count}");
                    foreach (Pop3MessageInfo msgInfo in messages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}, Size: {msgInfo.Size} bytes");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
