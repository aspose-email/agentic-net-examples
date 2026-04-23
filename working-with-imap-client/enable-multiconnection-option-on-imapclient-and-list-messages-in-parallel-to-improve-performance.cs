using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapMultiConnectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string host = "imap.example.com";
                string username = "username";
                string password = "password";

                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network call.");
                    return;
                }

                // Create and configure the ImapClient.
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Enable multi‑connection mode.
                        client.UseMultiConnection = MultiConnectionMode.Enable;

                        // Select the INBOX folder.
                        client.SelectFolder("INBOX");

                        // List messages asynchronously.
                        Task<ImapMessageInfoCollection> listTask = client.ListMessagesAsync();
                        ImapMessageInfoCollection messages = listTask.GetAwaiter().GetResult();

                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No messages found in INBOX.");
                            return;
                        }

                        // Fetch each message in parallel to improve performance.
                        List<Task<MailMessage>> fetchTasks = new List<Task<MailMessage>>();
                        foreach (ImapMessageInfo info in messages)
                        {
                            // Start fetching without awaiting immediately.
                            fetchTasks.Add(client.FetchMessageAsync(info.UniqueId));
                        }

                        // Wait for all fetch operations to complete.
                        Task.WaitAll(fetchTasks.ToArray());

                        // Process fetched messages.
                        foreach (Task<MailMessage> fetchTask in fetchTasks)
                        {
                            MailMessage message = fetchTask.Result;
                            Console.WriteLine($"Subject: {message.Subject}");
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
}
