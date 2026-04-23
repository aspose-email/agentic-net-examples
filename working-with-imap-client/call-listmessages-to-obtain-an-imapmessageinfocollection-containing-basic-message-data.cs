using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid external calls during CI
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Retrieve basic message information from the INBOX folder
                ImapMessageInfoCollection messages = client.ListMessagesAsync("INBOX", false, null)
                                                          .GetAwaiter()
                                                          .GetResult();

                Console.WriteLine($"Total messages: {messages.Count}");
                foreach (ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
