using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection details
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

            // Create and configure the POP3 client
            using (Pop3Client client = new Pop3Client())
            {
                try
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto; // Auto-detect security

                    // Implicitly connect by retrieving the message count
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages on server: {messageCount}");

                    // Delete the first two messages (if they exist)
                    int messagesToDelete = Math.Min(2, messageCount);
                    for (int i = 1; i <= messagesToDelete; i++)
                    {
                        client.DeleteMessage(i);
                        Console.WriteLine($"Message {i} marked for deletion.");
                    }

                    // Commit deletions – POP3 server will remove them after UPDATE state
                    Console.WriteLine("Deletion committed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
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
