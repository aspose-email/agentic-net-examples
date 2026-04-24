using Aspose.Email.Clients;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
            // Placeholder credentials/host guard
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";
            string zipPath = "archive.zip";

            if (host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Contains("example.com", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder server/credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string zipDirectory = Path.GetDirectoryName(Path.GetFullPath(zipPath));
            if (!string.IsNullOrEmpty(zipDirectory) && !Directory.Exists(zipDirectory))
            {
                try
                {
                    Directory.CreateDirectory(zipDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{zipDirectory}': {ex.Message}");
                    return;
                }
            }

            // Connect to IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the target folder
                    client.SelectFolder(folderName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder '{folderName}': {ex.Message}");
                    return;
                }

                // Retrieve list of messages in the folder
                ImapMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = await client.ListMessagesAsync(folderName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                if (messageInfos == null || messageInfos.Count == 0)
                {
                    Console.WriteLine("No messages found to archive.");
                    return;
                }

                // Fetch all messages asynchronously using their UniqueIds
                IList<string> uids = messageInfos.Select(info => info.UniqueId).ToList();
                IList<MailMessage> messages;
                try
                {
                    messages = await client.FetchMessagesAsync(uids);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch messages: {ex.Message}");
                    return;
                }

                // Create or overwrite the zip archive
                try
                {
                    using (FileStream zipStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                    {
                        for (int i = 0; i < messages.Count; i++)
                        {
                            MailMessage msg = messages[i];
                            // Use UniqueId as filename; fallback to a GUID if unavailable
                            string baseName = messageInfos[i].UniqueId ?? Guid.NewGuid().ToString();
                            string entryName = $"{baseName}.eml";

                            ZipArchiveEntry entry = archive.CreateEntry(entryName);
                            using (Stream entryStream = entry.Open())
                            {
                                // Save the message in EML format to the zip entry
                                msg.Save(entryStream, SaveOptions.DefaultEml);
                            }
                        }
                    }

                    Console.WriteLine($"Archived {messages.Count} messages to '{zipPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create zip archive: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
