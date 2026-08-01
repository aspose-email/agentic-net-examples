using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration (replace placeholders with real values when needed)
            string host = "pop.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.SSLImplicit;

            // Guard: skip network operations when placeholder credentials are detected
            bool isPlaceholder = host.Contains("example.com") ||
                                 username.Contains("example.com") ||
                                 password == "password";

            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder POP3 configuration detected. Skipping network operations.");
            }
            else
            {
                using (var pop3Client = new Pop3Client(host, port, username, password, security))
                {
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Message count: {messageCount}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
