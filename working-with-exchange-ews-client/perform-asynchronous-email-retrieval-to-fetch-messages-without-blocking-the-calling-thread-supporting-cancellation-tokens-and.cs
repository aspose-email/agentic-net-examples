using System;
using System.Collections.Generic;
using System.Net;
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
            await RunAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static async Task RunAsync()
    {
        // Placeholder connection settings
        string imapHost = "imap.example.com";
        int imapPort = 993;
        string imapUsername = "user@example.com";
        string imapPassword = "password";

        // Guard against executing real network calls with placeholder data
        if (imapHost.Contains("example.com"))
        {
            Console.WriteLine("Placeholder IMAP credentials detected. Skipping actual server call.");
            return;
        }

        // Create and connect the IMAP client
        ImapClient client = null;
        try
        {
            client = new ImapClient(imapHost, imapPort, imapUsername, imapPassword);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create IMAP client: {ex.Message}");
            return;
        }

        using (client)
        {
            // Cancellation token for async operations
            CancellationToken cancellationToken = CancellationToken.None;

            // List messages in the INBOX folder asynchronously
            ImapMessageInfoCollection messageInfos;
            try
            {
                messageInfos = await client.ListMessagesAsync("INBOX", cancellationToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                return;
            }

            // Process each message without blocking the calling thread
            foreach (ImapMessageInfo messageInfo in messageInfos)
            {
                try
                {
                    // Fetch the full message asynchronously
                    MailMessage message = await client.FetchMessageAsync(messageInfo.UniqueId, cancellationToken);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch message UID {messageInfo.UniqueId}: {ex.Message}");
                }
            }
        }
    }
}
