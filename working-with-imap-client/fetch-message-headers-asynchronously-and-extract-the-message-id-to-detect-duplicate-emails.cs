using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    await client.SelectFolderAsync("INBOX");

                    // Retrieve all messages in the selected folder
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync();

                    var seenMessageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (ImapMessageInfo info in messages)
                    {
                        // Prefer the Message-Id header; fall back to UniqueId if missing
                        string messageId = info.MessageId;
                        if (string.IsNullOrEmpty(messageId))
                        {
                            messageId = info.UniqueId;
                        }

                        if (!string.IsNullOrEmpty(messageId))
                        {
                            if (seenMessageIds.Contains(messageId))
                            {
                                Console.WriteLine($"Duplicate Message-Id detected: {messageId}");
                            }
                            else
                            {
                                seenMessageIds.Add(messageId);
                            }
                        }
                    }
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
