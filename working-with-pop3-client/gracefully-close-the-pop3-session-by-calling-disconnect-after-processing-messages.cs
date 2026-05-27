using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder POP3 settings detected. Skipping execution.");
                return;
            }

            // Create and connect the POP3 client.
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                // List messages in the mailbox.
                Pop3MessageInfoCollection messages = client.ListMessages();

                foreach (Pop3MessageInfo messageInfo in messages)
                {
                    // Fetch each message and display its subject.
                    using (MailMessage message = client.FetchMessage(messageInfo.SequenceNumber))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }

                // The using statement ensures the client is properly disposed (closed) after use.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
