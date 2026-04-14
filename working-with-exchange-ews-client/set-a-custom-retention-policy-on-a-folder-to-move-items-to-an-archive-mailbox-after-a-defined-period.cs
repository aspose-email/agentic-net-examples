using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values)
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                try
                {
                    // Define the folder to apply the retention policy (e.g., Inbox)
                    ExchangeFolderInfo folderInfo = client.GetFolderInfo("Inbox");
                    string folderUri = folderInfo.Uri;

                    // Define retention period (e.g., items older than 30 days)
                    int retentionDays = 30;
                    DateTime cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);

                    // Retrieve all messages in the folder
                    IList<ExchangeMessageInfo> messages = client.ListMessages(folderUri);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Use InternalDate for date comparison (per validation rules)
                        DateTime? internalDate = messageInfo.InternalDate;
                        if (internalDate.HasValue && internalDate.Value < cutoffDate)
                        {
                            // Archive the item to the user's archive mailbox
                            client.ArchiveItem(folderUri, messageInfo.UniqueUri);
                            Console.WriteLine($"Archived message: {messageInfo.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }
    }
}
