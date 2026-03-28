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
            // Initialize POP3 client with host, port, credentials, and security options
            using (Pop3Client client = new Pop3Client("pop.example.com", 110, "username", "password", SecurityOptions.Auto))
            {
                // Get mailbox information
                Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine($"Message count: {mailboxInfo.MessageCount}, Size: {mailboxInfo.OccupiedSize}");

                // List messages in the mailbox
                var messages = client.ListMessages();
                foreach (var info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");

                    // Fetch the full message using its sequence number
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Body: {message.Body}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
