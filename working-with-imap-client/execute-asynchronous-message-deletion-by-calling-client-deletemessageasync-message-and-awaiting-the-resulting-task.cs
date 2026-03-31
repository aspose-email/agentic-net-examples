using Aspose.Email.Clients;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailAsyncDelete
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard: skip real network calls when placeholders are used
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and use the IMAP client inside a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate credentials (synchronous version for simplicity)
                        client.ValidateCredentials();

                        // Retrieve the list of messages in the default folder
                        ImapMessageInfoCollection messages = await client.ListMessagesAsync();

                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No messages found in the mailbox.");
                            return;
                        }

                        // Select the first message to delete
                        ImapMessageInfo messageInfo = messages[0];
                        string uniqueId = messageInfo.UniqueId.ToString();

                        // Asynchronously delete the selected message
                        await client.DeleteMessageAsync(uniqueId);

                        // Commit the deletion (required for servers that support UIDPLUS)
                        await client.CommitDeletesAsync();

                        Console.WriteLine($"Message with UID {uniqueId} has been deleted.");
                    }
                    catch (Exception ex)
                    {
                        // Friendly error handling for client operations
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
