using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Placeholder values – replace with real credentials and service URL.
        string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";

        // Guard: skip network operations when placeholders are detected.
        if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.WriteLine("Placeholder credentials detected. Skipping archive operation.");
            return;
        }

        try
        {
            // Create EWS client and ensure proper disposal.
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Get mailbox information.
                ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                string inboxUri = mailboxInfo.InboxUri;

                // Retrieve all messages from the Inbox.
                ExchangeMessageInfoCollection allMessages = ewsClient.ListMessages(inboxUri);

                // Define the cutoff date for archiving (e.g., messages older than 6 months).
                DateTime cutoffDate = DateTime.Now.AddMonths(-6);

                // Collect messages older than the cutoff date.
                List<ExchangeMessageInfo> messagesToArchive = new List<ExchangeMessageInfo>();
                foreach (ExchangeMessageInfo msgInfo in allMessages)
                {
                    // Use InternalDate for the received time.
                    if (msgInfo.InternalDate < cutoffDate)
                    {
                        messagesToArchive.Add(msgInfo);
                    }
                }

                if (messagesToArchive.Count == 0)
                {
                    Console.WriteLine("No messages found older than the specified date.");
                    return;
                }

                // Prepare output directory for temporary EML files.
                string outputDir = "ArchivedEmails";
                if (!Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Prepare the ZIP archive path.
                string zipPath = Path.Combine(outputDir, "ArchivedEmails.zip");
                try
                {
                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare zip file: {ex.Message}");
                    return;
                }

                // Create ZIP archive and add each saved EML file.
                using (FileStream zipStream = new FileStream(zipPath, FileMode.Create, FileAccess.ReadWrite))
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    foreach (ExchangeMessageInfo msgInfo in messagesToArchive)
                    {
                        // Save the message locally as EML.
                        string emlPath = Path.Combine(outputDir, $"{Guid.NewGuid()}.eml");
                        try
                        {
                            ewsClient.SaveMessage(msgInfo.UniqueUri, emlPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message {msgInfo.UniqueUri}: {ex.Message}");
                            continue;
                        }

                        // Add the saved EML file to the ZIP archive.
                        try
                        {
                            zipArchive.CreateEntryFromFile(emlPath, Path.GetFileName(emlPath));
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add message {msgInfo.UniqueUri} to zip: {ex.Message}");
                        }

                        // Clean up the temporary EML file.
                        try
                        {
                            File.Delete(emlPath);
                        }
                        catch
                        {
                            // Ignored – best effort cleanup.
                        }
                    }
                }

                Console.WriteLine($"Archived {messagesToArchive.Count} messages to {zipPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
