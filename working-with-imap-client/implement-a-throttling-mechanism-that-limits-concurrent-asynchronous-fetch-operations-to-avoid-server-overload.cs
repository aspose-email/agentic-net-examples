using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;


namespace ThrottledImapFetch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder credentials – skip execution if they are not real.
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                    return;
                }

                // Limit concurrent fetches to avoid server overload.
                const int maxConcurrentFetches = 5;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrentFetches);

                // Create and configure the IMAP client.
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Attempt to validate credentials via a lightweight operation.
                        ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(10);
                        List<Task> fetchTasks = new List<Task>();

                        foreach (ImapMessageInfo messageInfo in messageInfos)
                        {
                            await semaphore.WaitAsync();

                            Task fetchTask = Task.Run(async () =>
                            {
                                try
                                {
                                    // Fetch the full message asynchronously.
                                    MailMessage message = await client.FetchMessageAsync(messageInfo.UniqueId);
                                    // Process the message (placeholder – here we just output the subject).
                                    Console.WriteLine($"Fetched: {message.Subject}");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Error fetching message UID {messageInfo.UniqueId}: {ex.Message}");
                                }
                                finally
                                {
                                    semaphore.Release();
                                }
                            });

                            fetchTasks.Add(fetchTask);
                        }

                        await Task.WhenAll(fetchTasks);
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
}
