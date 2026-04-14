using Aspose.Email.Storage.Pst;
using System;
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
            // Define connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Target folder (e.g., Inbox)
                string folderUri = client.MailboxInfo.InboxUri;

                // Retrieve folder information
                ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderUri);

                // ------------------------------------------------------------
                // Apply retention policy: delete items older than 30 days.
                // The actual API to set a retention policy may vary.
                // Typically you would set properties such as RetentionPeriod
                // and RetentionAction on the folderInfo and then update the folder.
                // Example (if supported):
                // folderInfo.RetentionPeriod = TimeSpan.FromDays(30);
                // folderInfo.RetentionAction = RetentionActionType.Delete;
                // client.UpdateFolder(folderInfo);
                // ------------------------------------------------------------

                Console.WriteLine("Retention policy would be applied to folder: " + folderInfo.DisplayName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
