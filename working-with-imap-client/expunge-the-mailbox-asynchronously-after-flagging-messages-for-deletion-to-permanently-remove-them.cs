using System;
using System.Collections.Generic;
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
            // Placeholder connection settings
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Select the INBOX folder
                await client.SelectFolderAsync("INBOX");

                // Retrieve all messages in the folder
                IList<ImapMessageInfo> messages = await client.ListMessagesAsync();

                // Flag each message for deletion without committing
                foreach (ImapMessageInfo msgInfo in messages)
                {
                    // DeleteMessageAsync(uid, commitNow) – commitNow = false to only flag
                    await client.DeleteMessageAsync(msgInfo.UniqueId, false);
                }

                // Expunge (permanently remove) the flagged messages
                // UnselectFolderAsync(doNotExpunge) – false means expunge
                await client.UnselectFolderAsync(false);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
