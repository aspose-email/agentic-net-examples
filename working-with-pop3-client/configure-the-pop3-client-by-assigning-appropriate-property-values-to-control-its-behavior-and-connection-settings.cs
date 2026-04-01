using System;
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
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (host.Contains("example") || username.Contains("example"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 client configuration.");
                return;
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client())
            {
                // Basic connection properties
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;

                // Security and authentication settings
                client.SecurityOptions = SecurityOptions.Auto;
                client.AllowedAuthentication = Pop3KnownAuthenticationType.Login |
                                                Pop3KnownAuthenticationType.APOP;

                // Additional behavior controls
                client.UsePipelining = true;          // Enable pipelining
                client.Timeout = 60000;               // Timeout in milliseconds

                // Validate credentials and retrieve mailbox info
                try
                {
                    client.ValidateCredentials();
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Messages: {mailboxInfo.MessageCount}, Size: {mailboxInfo.OccupiedSize}");
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
