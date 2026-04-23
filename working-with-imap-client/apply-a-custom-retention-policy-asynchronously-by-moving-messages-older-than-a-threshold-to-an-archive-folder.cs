using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Configuration
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string sourceFolder = "INBOX";
            string archiveFolder = "Archive";
            int retentionDays = 30;

            // Guard against placeholder credentials
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Ensure the archive folder exists
                bool archiveExists = await client.ExistFolderAsync(archiveFolder);
                if (!archiveExists)
                {
                    await client.CreateFolderAsync(archiveFolder);
                }

                // Select the source folder
                await client.SelectFolderAsync(sourceFolder);

                // Retrieve all messages in the source folder
                IEnumerable<ImapMessageInfo> allMessages = await client.ListMessagesAsync(sourceFolder);
                DateTime cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
                List<string> uidsToArchive = new List<string>();

                foreach (ImapMessageInfo messageInfo in allMessages)
                {
                    if (messageInfo.InternalDate < cutoffDate)
                    {
                        uidsToArchive.Add(messageInfo.UniqueId);
                    }
                }

                if (uidsToArchive.Count > 0)
                {
                    // Move the selected messages to the archive folder
                    await client.MoveMessagesAsync(uidsToArchive, archiveFolder, CancellationToken.None);
                    Console.WriteLine($"{uidsToArchive.Count} message(s) moved to '{archiveFolder}'.");
                }
                else
                {
                    Console.WriteLine("No messages found that match the retention criteria.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
