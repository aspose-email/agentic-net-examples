using Aspose.Email.Tools.Search;
using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

namespace ImapUidSyncExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Placeholder credentials detection (skip real network calls in CI)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and use the IMAP client inside a using block
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Select the INBOX folder (asynchronously)
                        await client.SelectFolderAsync("INBOX");

                        // Retrieve all messages – an empty MailQuery represents “all”
                        ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(new MailQueryBuilder().GetQuery());

                        // Collect UID values
                        List<string> uidValues = new List<string>();
                        foreach (ImapMessageInfo info in messageInfos)
                        {
                            uidValues.Add(info.UniqueId);
                        }

                        // Simulate storing UIDs in a local database
                        Console.WriteLine($"Retrieved {uidValues.Count} message UID(s).");
                        foreach (string uid in uidValues)
                        {
                            Console.WriteLine(uid);
                        }
                    }
                    catch (Exception imapEx)
                    {
                        // Friendly error for IMAP operations
                        Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Global exception handling
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
