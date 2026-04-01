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
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder POP3 credentials detected. Skipping connection.");
                return;
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // List messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    Console.WriteLine($"Total messages: {messages.Count}");

                    foreach (Pop3MessageInfo info in messages)
                    {
                        string from = (info.From != null && info.From.Count > 0) ? info.From[0].Address : "Unknown";
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {from}");
                        Console.WriteLine($"Date: {info.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
