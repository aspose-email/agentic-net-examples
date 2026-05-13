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
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping network operations.");
                return;
            }

            // Create the IMAP client (implements IAsyncImapClient)
            using (ImapClient client = new ImapClient(host, 993, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the source folder
                    await client.SelectFolderAsync("INBOX");

                    // Ensure the destination folder exists
                    bool archiveExists = await client.ExistFolderAsync("Archive");
                    if (!archiveExists)
                    {
                        await client.CreateFolderAsync("Archive");
                    }

                    // Retrieve all messages from the source folder
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync();

                    foreach (ImapMessageInfo messageInfo in messageInfos)
                    {
                        // Preserve original flags
                        ImapMessageFlags originalFlags = messageInfo.Flags;

                        // Copy the message to the Archive folder
                        string newUid = await client.CopyMessageAsync(messageInfo.UniqueId, "Archive");

                        // If the server returned a UID for the copied message, reapply the original flags
                        if (!string.IsNullOrEmpty(newUid))
                        {
                            await client.AddMessageFlagsAsync(newUid, originalFlags);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
