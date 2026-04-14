using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
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
            // Placeholder credentials – replace with real values or skip execution.
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Create EWS client inside a using block.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                try
                {
                    // Get information about the Inbox folder.
                    ExchangeFolderInfo inboxInfo = client.GetFolderInfo("Inbox");

                    // Ensure the custom archive folder exists; create it if necessary.
                    string archiveFolderName = "ArchiveFolder";
                    ExchangeFolderInfo archiveInfo;
                    if (!client.FolderExists(inboxInfo.Uri, archiveFolderName, out archiveInfo))
                    {
                        archiveInfo = client.CreateFolder(inboxInfo.Uri, archiveFolderName);
                    }

                    // Retrieve message URIs from the Inbox.
                    IList<ExchangeMessageInfo> messageInfos = client.ListMessages(inboxInfo.Uri);
                    int copiedCount = 0;

                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        if (copiedCount >= 100)
                            break;

                        // Copy each message to the archive folder.
                        client.CopyItem(messageInfo.UniqueUri, archiveInfo.Uri);
                        copiedCount++;
                    }

                    Console.WriteLine($"{copiedCount} messages copied to '{archiveFolderName}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
