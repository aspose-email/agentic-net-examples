using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network call in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping connection.");
                return;
            }

            // Create an asynchronous IMAP client with default settings
            IAsyncImapClient asyncClient = await ImapClient.CreateAsync(host, username, null);
            ImapClient client = asyncClient as ImapClient;

            if (client == null)
            {
                Console.Error.WriteLine("Failed to cast IAsyncImapClient to ImapClient.");
                return;
            }

            // Ensure the client is disposed properly
            using (client)
            {
                // Validate credentials (wrapped in try/catch by outer block)
                bool isValid = await client.ValidateCredentialsAsync();
                Console.WriteLine($"Credentials valid: {isValid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
