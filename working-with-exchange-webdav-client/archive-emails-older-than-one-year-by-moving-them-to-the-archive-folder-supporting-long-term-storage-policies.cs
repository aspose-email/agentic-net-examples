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
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Validate connectivity by attempting to list messages in the Inbox.
                    ExchangeMessageInfoCollection inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Retrieve the Archive folder information.
                    ExchangeFolderInfo archiveFolder = client.GetFolderInfo("Archive");
                    string archiveFolderUri = archiveFolder.Uri;

                    DateTime cutoffDate = DateTime.UtcNow.AddYears(-1);

                    foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                    {
                        // Use InternalDate for the message's received time.
                        DateTime? internalDate = messageInfo.InternalDate;
                        if (internalDate.HasValue && internalDate.Value < cutoffDate)
                        {
                            // Move the message to the Archive folder.
                            client.MoveMessage(messageInfo, archiveFolderUri);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during archiving operation: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
