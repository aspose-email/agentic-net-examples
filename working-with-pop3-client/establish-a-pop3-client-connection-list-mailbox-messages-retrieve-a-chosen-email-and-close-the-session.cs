using System;
using Aspose.Email;
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

            // Skip real network call when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // List messages in the mailbox
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    Console.WriteLine($"Total messages: {messages.Count}");
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Seq: {info.SequenceNumber}, Subject: {info.Subject}");
                    }

                    // Retrieve the first message if any
                    if (messages.Count > 0)
                    {
                        int firstSeq = messages[0].SequenceNumber;
                        using (MailMessage message = client.FetchMessage(firstSeq))
                        {
                            Console.WriteLine("Fetched message details:");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"Body: {message.Body}");
                        }
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
