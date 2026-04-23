using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Measure round‑trip time for ListMessagesAsync
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Select the INBOX folder (required before listing messages)
                client.SelectFolder("INBOX");

                Stopwatch sw = Stopwatch.StartNew();

                // Retrieve the list of messages in the selected folder
                ImapMessageInfoCollection messages = await client.ListMessagesAsync();

                sw.Stop();

                Console.WriteLine($"Fetched {messages.Count} messages in {sw.ElapsedMilliseconds} ms.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
