using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange;
class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string exchangeUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(exchangeUri, username, password))
            {
                // Parent folder (Inbox) URI
                string parentFolderUri = client.MailboxInfo.InboxUri;

                // Target folder name
                string targetFolderName = "Classified";


                // Skip external calls when placeholder credentials are used
                if (exchangeUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure the target folder exists
                string targetFolderUri;
                if (!client.FolderExists(parentFolderUri, targetFolderName))
                {
                    ExchangeFolderInfo createdFolder = client.CreateFolder(parentFolderUri, targetFolderName);
                    targetFolderUri = createdFolder.Uri;
                }
                else
                {
                    // Retrieve existing folder info
                    ExchangeFolderInfo existingFolder = client.GetFolderInfo(parentFolderUri + "/" + targetFolderName);
                    targetFolderUri = existingFolder.Uri;
                }

                // Define filter strings for classification
                string[] filter = new string[] { "Invoice", "Payment" };

                // Create the inbox rule that moves matching messages
                InboxRule rule = InboxRule.CreateRuleMoveContaining(filter, targetFolderUri);
                rule.DisplayName = "Move invoices to Classified";
                rule.IsEnabled = true;

                // Add the rule to the mailbox
                client.CreateInboxRule(rule);

                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
