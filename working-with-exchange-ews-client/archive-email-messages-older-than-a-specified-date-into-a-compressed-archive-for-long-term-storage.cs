using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            DateTime archiveBeforeDate = new DateTime(2022, 1, 1);
            string zipPath = "ArchivedEmails.zip";

            // Ensure the directory for the zip file exists
            try
            {
                string zipDir = Path.GetDirectoryName(Path.GetFullPath(zipPath));
                if (!Directory.Exists(zipDir))
                {
                    Directory.CreateDirectory(zipDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare archive directory: {ex.Message}");
                return;
            }

            // Create or open the zip archive
            using (FileStream zipStream = new FileStream(zipPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
            {
                // Connect to Exchange using EWS
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                    {
                        // List messages in the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            // Filter messages older than the specified date
                            if (info.Date < archiveBeforeDate)
                            {
                                // Fetch the full message
                                MailMessage message = client.FetchMessage(info.UniqueUri);
                                // Prepare a safe file name for the entry
                                string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : MakeFileNameSafe(message.Subject);
                                string entryName = $"{safeSubject}_{info.UniqueUri}.eml";

                                // Save the message into the zip archive
                                using (MemoryStream msgStream = new MemoryStream())
                                {
                                    message.Save(msgStream, SaveOptions.DefaultEml);
                                    msgStream.Position = 0;
                                    ZipArchiveEntry entry = archive.CreateEntry(entryName);
                                    using (Stream entryStream = entry.Open())
                                    {
                                        msgStream.CopyTo(entryStream);
                                    }
                                }

                                // Optionally, delete or move the original message after archiving
                                // client.DeleteMessage(info.UniqueUri, true); // move to Deleted Items
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to replace invalid filename characters
    private static string MakeFileNameSafe(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
