using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Sender address to filter
            string senderAddress = "spam@example.com";

            // Name of the quarantine folder
            string quarantineFolderName = "Quarantine";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || senderAddress.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Ensure the quarantine folder exists (create if missing)
                ExchangeFolderInfo quarantineFolderInfo;
                if (!client.FolderExists(client.MailboxInfo.InboxUri, quarantineFolderName, out quarantineFolderInfo))
                {
                    quarantineFolderInfo = client.CreateFolder(client.MailboxInfo.InboxUri, quarantineFolderName);
                }

                // List all messages in the Inbox
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Move messages from the specified sender to the quarantine folder
                foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                {
                    if (messageInfo.From != null &&
                        string.Equals(messageInfo.From.Address, senderAddress, StringComparison.OrdinalIgnoreCase))
                    {
                        client.MoveItem(messageInfo.UniqueUri, quarantineFolderInfo.Uri);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
