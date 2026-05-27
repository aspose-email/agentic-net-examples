using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP credentials detected. Skipping processing.");
                return;
            }

            // Create the IMAP client and ensure it is disposed properly
            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Validate the connection; on failure, report and exit gracefully
                try
                {
                    await client.ValidateCredentialsAsync();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP connection failed: {ex.Message}");
                    return;
                }

                // Select the INBOX folder
                await client.SelectFolderAsync("INBOX");

                // Retrieve the list of messages asynchronously
                ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync();

                // Process each message that meets the size threshold (>= 1 KB)
                foreach (ImapMessageInfo info in messageInfos)
                {
                    if (info.Size < 1024) // Skip small messages
                        continue;

                    // Fetch the full message asynchronously
                    MailMessage message = await client.FetchMessageAsync(info.UniqueId);

                    // Example processing: output the subject to the console
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
