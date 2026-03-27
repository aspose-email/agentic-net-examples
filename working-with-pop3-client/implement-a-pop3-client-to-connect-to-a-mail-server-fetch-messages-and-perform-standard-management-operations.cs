using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server configuration
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Create and use the POP3 client
                try
                {
                    using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                    {
                        // Verify connection
                        try
                        {
                            client.Noop();
                        }
                        catch (Exception connEx)
                        {
                            Console.Error.WriteLine($"Failed to connect to POP3 server: {connEx.Message}");
                            return;
                        }

                        // List messages on the server
                        Pop3MessageInfoCollection messageInfos = client.ListMessages();

                        Console.WriteLine($"Total messages: {messageInfos.Count}");

                        foreach (Pop3MessageInfo info in messageInfos)
                        {
                            // Fetch full message
                            MailMessage message = client.FetchMessage(info.SequenceNumber);
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {info.Date}");
                            Console.WriteLine(new string('-', 40));

                            // Delete the message after processing
                            client.DeleteMessage(info.SequenceNumber);
                        }

                        // Commit deletions
                        client.CommitDeletes();
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
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
