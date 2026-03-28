using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // POP3 server connection details
            string host = "pop.example.com";
            int port = 110; // use 995 for SSL
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client (client variable name must be preserved)
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Client connection safety guard
                try
                {
                    // Retrieve total number of messages in the mailbox
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");

                    if (messageCount > 0)
                    {
                        // Fetch the first message (sequence number starts at 1)
                        using (MailMessage message = client.FetchMessage(1))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Body: {message.Body}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
