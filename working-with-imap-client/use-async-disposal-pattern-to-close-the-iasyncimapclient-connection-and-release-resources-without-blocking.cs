using Aspose.Email.Clients;
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
            // Placeholder credentials – skip actual network call in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping execution.");
                return;
            }

            // Create the IMAP client (synchronous constructor) and use it within a using block.
            // The client implements IDisposable, so it will be disposed when the block ends.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Validate credentials using a lightweight async operation.
                bool credentialsValid = await client.ValidateCredentialsAsync();
                if (!credentialsValid)
                {
                    Console.Error.WriteLine("IMAP credentials validation failed.");
                    return;
                }

                // Select the INBOX folder asynchronously.
                await client.SelectFolderAsync("INBOX");

                // Retrieve up to 10 messages from the selected folder.
                ImapMessageInfoCollection messages = await client.ListMessagesAsync(10);
                Console.WriteLine($"Fetched {messages.Count} message(s) from INBOX.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
