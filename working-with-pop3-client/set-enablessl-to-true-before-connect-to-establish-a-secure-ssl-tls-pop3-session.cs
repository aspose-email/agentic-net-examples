using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients;
using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string host = "pop.example.com";
            int port = 995;
            string username = "username";
            string password = "password";

            // Guard: skip network calls when placeholder values are present
            bool hasPlaceholders = host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                                   username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                   password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (hasPlaceholders)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error retrieving message count: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
