using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapCleanup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    Console.CancelKeyPress += (sender, e) =>
                    {
                        e.Cancel = true;
                        cts.Cancel();
                    };

                    await RunCleanupLoopAsync(cts.Token);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static async Task RunCleanupLoopAsync(CancellationToken token)
        {
            const string host = "imap.example.com";
            const string username = "user@example.com";
            const string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP settings detected. Skipping cleanup task.");
                return;
            }

            while (!token.IsCancellationRequested)
            {
                await CleanupDeletedMessagesAsync(host, username, password, token);
                try
                {
                    await Task.Delay(TimeSpan.FromHours(1), token);
                }
                catch (TaskCanceledException)
                {
                    // Loop exit requested.
                }
            }
        }

        private static async Task CleanupDeletedMessagesAsync(string host, string username, string password, CancellationToken token)
        {
            try
            {
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Select the folder to clean up.
                    client.SelectFolder("INBOX");

                    // Retrieve information about all messages in the folder.
                    IList<ImapMessageInfo> allMessages = client.ListMessages();

                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    DateTime thresholdDate = DateTime.UtcNow.AddDays(-30);

                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        if (messageInfo.Flags.HasFlag(ImapMessageFlags.Deleted) && messageInfo.InternalDate < thresholdDate)
                        {
                            messagesToDelete.Add(messageInfo);
                        }
                    }

                    if (messagesToDelete.Count > 0)
                    {
                        // Mark messages for deletion and commit the changes.
                        await client.DeleteMessagesAsync(messagesToDelete, token);
                        await client.CommitDeletesAsync(token);
                        Console.WriteLine($"{messagesToDelete.Count} messages permanently deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages older than 30 days marked for deletion were found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Cleanup error: {ex.Message}");
            }
        }
    }
}
