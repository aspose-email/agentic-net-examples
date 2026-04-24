using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual use
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip network calls when placeholders are detected
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create the IMAP client within a using block to ensure disposal
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Enable compression if the server supports the COMPRESS extension
                    client.CompressSupported = true;

                    // Validate credentials asynchronously – this establishes the connection
                    await client.ValidateCredentialsAsync();

                    // Example async operation: list messages in the INBOX folder
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync(
                        folderName: "INBOX",
                        modificationSequence: 0,
                        retrieveRecursively: false,
                        messageExtraFields: null,
                        connection: null,
                        token: CancellationToken.None);

                    Console.WriteLine($"Total messages in INBOX: {messages.Count}");
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
