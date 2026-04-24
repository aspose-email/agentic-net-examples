using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

namespace ImapCustomFlagExample
{
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

                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Skipping IMAP operation due to placeholder credentials.");
                    return;
                }

                // Cancellation token for async operations
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    CancellationToken token = cts.Token;

                    // Create and connect the IMAP client
                    using (ImapClient client = new ImapClient(host, username, password))
                    {
                        // Select the INBOX folder
                        await client.SelectFolderAsync("INBOX", token);

                        // Retrieve all messages in the folder
                        IEnumerable<ImapMessageInfo> messages = await client.ListMessagesAsync("INBOX", token);

                        // Define the subject pattern to match
                        string subjectPattern = "Important";

                        // Iterate over messages and add a custom flag where the subject matches the pattern
                        foreach (ImapMessageInfo messageInfo in messages)
                        {
                            if (messageInfo.Subject != null && messageInfo.Subject.Contains(subjectPattern))
                            {
                                // Create a custom keyword flag
                                ImapMessageFlags customFlag = ImapMessageFlags.Keyword("CustomFlag");

                                // Add the custom flag to the message identified by its UniqueId
                                await client.AddMessageFlagsAsync(
                                    connection: null,
                                    uniqueId: messageInfo.UniqueId,
                                    flags: customFlag,
                                    token: token);
                            }
                        }

                        Console.WriteLine("Custom flag processing completed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
