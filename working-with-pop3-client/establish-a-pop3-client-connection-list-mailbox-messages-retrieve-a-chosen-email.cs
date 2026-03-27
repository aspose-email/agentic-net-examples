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
            // Connection parameters (replace with real values)
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Create and dispose the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // List messages in the mailbox
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    Console.WriteLine($"Total messages: {messages.Count}");

                    if (messages.Count > 0)
                    {
                        // Retrieve the first message's info
                        Pop3MessageInfo firstInfo = messages[0];

                        // Fetch the full message using its sequence number
                        using (MailMessage mail = client.FetchMessage(firstInfo.SequenceNumber))
                        {
                            Console.WriteLine($"Subject: {mail.Subject}");
                            Console.WriteLine($"From: {mail.From}");
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
