using Aspose.Email.Clients;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace AsyncPop3Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // POP3 server configuration
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate credentials asynchronously
                        await client.ValidateCredentialsAsync();

                        // Get total message count asynchronously
                        int messageCount = await client.GetMessageCountAsync();
                        Console.WriteLine($"Total messages: {messageCount}");

                        // List messages asynchronously
                        Pop3MessageInfoCollection messages = await client.ListMessagesAsync();

                        foreach (Pop3MessageInfo messageInfo in messages)
                        {
                            // Fetch each message asynchronously
                            using (MailMessage message = await client.FetchMessageAsync(messageInfo.SequenceNumber))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
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
}
