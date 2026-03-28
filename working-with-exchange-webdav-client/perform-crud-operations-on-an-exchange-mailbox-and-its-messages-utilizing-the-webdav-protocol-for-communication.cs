using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Get mailbox information
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // Define the target folder (a subfolder of Inbox)
                string parentFolderUri = mailboxInfo.InboxUri;
                string newFolderName = "TestFolder";
                string newFolderUri = $"{parentFolderUri}/{newFolderName}";

                // Ensure the folder does not already exist, then create it
                if (!client.FolderExists(parentFolderUri, newFolderName, out ExchangeFolderInfo _))
                {
                    client.CreateFolder(parentFolderUri, newFolderName);
                }

                // Create a simple email message
                MailMessage message = new MailMessage(
                    "from@example.com",
                    "to@example.com",
                    "Test Subject",
                    "This is a test message created via Aspose.Email.");

                // Append the message to the newly created folder
                client.AppendMessage(newFolderUri, message);

                // List messages in the folder
                ExchangeMessageInfoCollection messages = client.ListMessages(newFolderUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message to read its properties
                    MailMessage fetched = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine($"Fetched Message Subject: {fetched.Subject}");
                }

                // Delete the first message if any exist
                if (messages.Count > 0)
                {
                    client.DeleteMessage(messages[0].UniqueUri);
                }

                // Delete the test folder
                client.DeleteFolder(newFolderUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
