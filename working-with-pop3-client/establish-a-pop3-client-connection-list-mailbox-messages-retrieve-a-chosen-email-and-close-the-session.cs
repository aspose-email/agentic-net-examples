using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection parameters (replace with real values)
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                try
                {
                    // Initialize and connect the POP3 client
                    using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                    {
                        // List messages in the mailbox
                        Pop3MessageInfoCollection messages = client.ListMessages();
                        Console.WriteLine($"Total messages: {messages.Count}");

                        if (messages.Count > 0)
                        {
                            // Retrieve information about the first message
                            Pop3MessageInfo firstInfo = messages[0];

                            // Fetch the full message using its sequence number
                            using (MailMessage message = client.FetchMessage(firstInfo.SequenceNumber))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"Date: {message.Date}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
