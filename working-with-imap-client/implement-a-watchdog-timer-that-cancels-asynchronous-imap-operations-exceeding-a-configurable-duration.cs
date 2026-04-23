using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            int timeoutSeconds = 10;

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping execution.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
                    {
                        Task<ImapMessageInfoCollection> listTask = client.ListMessagesAsync(0, cts.Token);
                        ImapMessageInfoCollection messages = listTask.GetAwaiter().GetResult();
                        Console.WriteLine($"Retrieved {messages.Count} messages.");
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.Error.WriteLine($"IMAP operation timed out after {timeoutSeconds} seconds.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
