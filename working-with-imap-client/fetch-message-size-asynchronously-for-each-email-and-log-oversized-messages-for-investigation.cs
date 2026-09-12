using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace Sample
{
    class Program
    {
        // Threshold for oversized messages (5 MB)
        private const long OversizedThreshold = 5 * 1024 * 1024;

        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder values to avoid unwanted network calls.
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and connect the IMAP client inside a using block.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate credentials by selecting the INBOX folder.
                        await client.SelectFolderAsync("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                        return;
                    }

                    // Retrieve the list of messages in the INBOX folder asynchronously.
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync("INBOX");

                    // Iterate through each message and check its size.
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        long size = messageInfo.Size;
                        if (size > OversizedThreshold)
                        {
                            Console.WriteLine($"Oversized message detected: UID={messageInfo.UniqueId}, Size={size} bytes, Subject=\"{messageInfo.Subject}\"");
                        }
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
