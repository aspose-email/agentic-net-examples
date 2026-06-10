using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server connection parameters (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example") || username.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create and use the POP3 client inside a using block to ensure disposal
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (network operation)
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                try
                {
                    // List messages in the mailbox
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
