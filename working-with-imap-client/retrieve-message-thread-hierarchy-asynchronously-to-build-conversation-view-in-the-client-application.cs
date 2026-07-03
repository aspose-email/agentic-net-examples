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
            // Placeholder credentials – skip actual network call in CI environments.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Skipping IMAP operations because placeholder credentials are used.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder.
                    await client.SelectFolderAsync("INBOX", null, CancellationToken.None);

                    // Retrieve message threads using default search conditions.
                    ThreadSearchConditions conditions = new ThreadSearchConditions();
                    List<MessageThreadResult> threads = await client.GetMessageThreadsAsync(conditions);

                    // Display thread information.
                    foreach (MessageThreadResult thread in threads)
                    {
                        // ConversationId identifies the thread.
                        string conversationId = thread.ConversationId;
                        // ChildMessages contains the messages belonging to the thread.
                        int messageCount = thread.ChildMessages != null ? thread.ChildMessages.Count : 0;

                        Console.WriteLine($"Thread ConversationId: {conversationId}, Messages: {messageCount}");
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
