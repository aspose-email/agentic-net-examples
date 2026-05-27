using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – real credentials should be provided.
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected to avoid runtime failures.
            if (serverUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Connect to the Exchange server using WebDAV client.
            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                try
                {
                    // Retrieve the URI of the "Important" folder (creates it if necessary).
                    ExchangeFolderInfo importantFolderInfo = client.GetFolderInfo("Important");
                    string importantFolderUri = importantFolderInfo?.Uri;

                    if (string.IsNullOrEmpty(importantFolderUri))
                    {
                        Console.Error.WriteLine("Unable to locate the 'Important' folder.");
                        return;
                    }

                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Move each message to the "Important" folder.
                    foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                    {
                        try
                        {
                            client.MoveMessage(messageInfo, importantFolderUri);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to move message '{messageInfo?.Subject}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
