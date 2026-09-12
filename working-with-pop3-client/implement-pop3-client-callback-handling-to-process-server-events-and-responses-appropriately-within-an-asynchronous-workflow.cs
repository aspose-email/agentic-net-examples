using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static async Task Main()
    {
        try
        {
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            IAsyncPop3Client pop3Client = await Pop3Client.CreateAsync(
                host,
                username,
                null,
                port,
                SecurityOptions.Auto,
                CancellationToken.None);

            try
            {
                Pop3MessageInfoCollection messageInfos = await pop3Client.ListMessagesAsync();

                foreach (Pop3MessageInfo info in messageInfos)
                {
                    MailMessage message = await pop3Client.FetchMessageAsync(info.SequenceNumber);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
            finally
            {
                pop3Client.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
