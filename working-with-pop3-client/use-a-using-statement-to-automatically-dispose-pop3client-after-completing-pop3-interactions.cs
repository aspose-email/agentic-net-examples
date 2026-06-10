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
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping connection.");
                return;
            }

            // Automatically dispose the POP3 client after use
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (connection is established on first operation)
                    client.ValidateCredentials();

                    // Retrieve list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
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
