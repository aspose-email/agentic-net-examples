using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Retrieve connection settings from environment variables.
            string mailboxUri = Environment.GetEnvironmentVariable("EXCHANGE_MAILBOX_URI");
            string username = Environment.GetEnvironmentVariable("EXCHANGE_USERNAME");
            string password = Environment.GetEnvironmentVariable("EXCHANGE_PASSWORD");

            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Missing Exchange connection settings. Set EXCHANGE_MAILBOX_URI, EXCHANGE_USERNAME, and EXCHANGE_PASSWORD environment variables.");
                return;
            }

            // Create the async EWS client.
            IAsyncEwsClient client;
            try
            {
                client = await EWSClient.GetEwsClientAsync(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Prepare dummy journal entries as MailMessage objects.
            List<MailMessage> journalEntries = new List<MailMessage>();
            for (int i = 1; i <= 25; i++)
            {
                MailMessage msg = new MailMessage
                {
                    From = "journal@example.com",
                    To = "archive@example.com",
                    Subject = $"Journal Entry {i}",
                    Body = $"This is the body of journal entry {i}."
                };
                journalEntries.Add(msg);
            }

            const int batchSize = 10;
            int totalInserted = 0;

            // Progress reporter.
            IProgress<int> progress = new Progress<int>(p => Console.WriteLine($"Inserted {p} journal entries."));

            // Insert entries in batches.
            for (int offset = 0; offset < journalEntries.Count; offset += batchSize)
            {
                List<MailMessage> batch = journalEntries.GetRange(offset, Math.Min(batchSize, journalEntries.Count - offset));

                // Build the AppendMessage request.
                EwsAppendMessage appendMessage = EwsAppendMessage.Create()
                    .AddMessages(batch)
                    .SetFolder("journal"); // Target folder name; adjust as needed.

                try
                {
                    IEnumerable<string> result = await client.AppendMessagesAsync(appendMessage);
                    int inserted = 0;
                    foreach (var _ in result) inserted++;
                    totalInserted += inserted;
                    progress.Report(totalInserted);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Batch insertion failed: {ex.Message}");
                    // Continue with next batch.
                }
            }

            Console.WriteLine($"Batch insertion completed. Total journal entries inserted: {totalInserted}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
