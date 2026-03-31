using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder POP3 server details
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Skipping POP3 connection due to placeholder credentials.");
                return;
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Retrieve the first message (sequence number 1)
                    using (MailMessage message = client.FetchMessage(1))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + message.From);
                        Console.WriteLine("Body Preview: " + (message.Body?.Substring(0, Math.Min(100, message.Body.Length)) ?? string.Empty));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error retrieving message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
