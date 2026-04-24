using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping backup operation.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = "Backup";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Retrieve all message infos in the folder
                ImapMessageInfoCollection allMessageInfos;
                try
                {
                    allMessageInfos = client.ListMessages("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                const int batchSize = 500;
                int totalMessages = allMessageInfos.Count;
                Console.WriteLine($"Total messages to process: {totalMessages}");

                for (int start = 0; start < totalMessages; start += batchSize)
                {
                    int count = Math.Min(batchSize, totalMessages - start);
                    List<ImapMessageInfo> batchInfos = new List<ImapMessageInfo>();
                    for (int i = 0; i < count; i++)
                    {
                        batchInfos.Add(allMessageInfos[start + i]);
                    }

                    // Process each message in the current batch
                    foreach (ImapMessageInfo messageInfo in batchInfos)
                    {
                        MailMessage message;
                        try
                        {
                            message = client.FetchMessage(messageInfo.UniqueId);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch message UID {messageInfo.UniqueId}: {ex.Message}");
                            continue;
                        }

                        string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                        // Replace invalid filename characters
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string emlPath = Path.Combine(outputDirectory, $"{messageInfo.UniqueId}_{safeSubject}.eml");
                        try
                        {
                            message.Save(emlPath, SaveOptions.DefaultEml);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message UID {messageInfo.UniqueId} to file: {ex.Message}");
                        }
                    }

                    Console.WriteLine($"Processed batch {start / batchSize + 1} ({count} messages).");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
