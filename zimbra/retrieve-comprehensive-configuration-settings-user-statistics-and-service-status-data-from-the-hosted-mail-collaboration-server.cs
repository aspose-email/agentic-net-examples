using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping live server interaction.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine("Mailbox URIs:");
                Console.WriteLine($"Inbox: {mailboxInfo.InboxUri}");
                Console.WriteLine($"Sent Items: {mailboxInfo.SentItemsUri}");
                Console.WriteLine($"Drafts: {mailboxInfo.DraftsUri}");

                string versionInfo = client.GetVersionInfo();
                Console.WriteLine($"Exchange Server Version: {versionInfo}");

                Contact[] mailboxes = client.GetMailboxes();
                Console.WriteLine($"Total mailboxes retrieved: {mailboxes.Length}");
                foreach (Contact contact in mailboxes)
                {
                    Console.WriteLine($"- {contact.DisplayName}");
                }

                ExchangeFolderInfo inboxInfo = client.GetFolderInfo("inbox");
                Console.WriteLine($"Inbox Folder URI: {inboxInfo.Uri}");
                Console.WriteLine($"Item Count: {inboxInfo.TotalCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
