using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Cut‑off date: messages older than this will be archived
            DateTime cutoffDate = new DateTime(2022, 1, 1);

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // List all messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Prepare a temporary folder for storing messages before compression
                    string tempFolder = Path.Combine(Path.GetTempPath(), "ArchiveTemp");
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Archive only messages older than the specified date
                        if (info.InternalDate < cutoffDate)
                        {
                            // Save the message to a local .eml file
                            string safeSubject = string.IsNullOrEmpty(info.Subject) ? "NoSubject" : info.Subject;
                            string fileName = $"{safeSubject}_{info.MessageId}.eml";
                            string filePath = Path.Combine(tempFolder, fileName);

                            // Guard file write operation
                            try
                            {
                                client.SaveMessage(info.UniqueUri, filePath);
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to save message '{info.Subject}': {ioEx.Message}");
                                continue;
                            }

                            // Move the message to the archive mailbox
                            try
                            {
                                client.ArchiveItem(client.MailboxInfo.InboxUri, info.UniqueUri);
                            }
                            catch (Exception archiveEx)
                            {
                                Console.Error.WriteLine($"Failed to archive message '{info.Subject}': {archiveEx.Message}");
                            }
                        }
                    }

                    // Create a compressed ZIP archive of the saved messages
                    string zipPath = "ArchivedMessages.zip";
                    if (File.Exists(zipPath))
                    {
                        try
                        {
                            File.Delete(zipPath);
                        }
                        catch (Exception delEx)
                        {
                            Console.Error.WriteLine($"Unable to delete existing archive: {delEx.Message}");
                        }
                    }

                    try
                    {
                        ZipFile.CreateFromDirectory(tempFolder, zipPath);
                    }
                    catch (Exception zipEx)
                    {
                        Console.Error.WriteLine($"Failed to create ZIP archive: {zipEx.Message}");
                    }

                    // Clean up the temporary folder
                    try
                    {
                        Directory.Delete(tempFolder, true);
                    }
                    catch (Exception cleanEx)
                    {
                        Console.Error.WriteLine($"Failed to clean temporary files: {cleanEx.Message}");
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
