using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox connection parameters (replace with actual values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Output base directory for exported messages
            string outputRoot = "ExportedMailbox";

            // Ensure the output root directory exists
            if (!Directory.Exists(outputRoot))
            {
                Directory.CreateDirectory(outputRoot);
            }

            // Create and use the EWS client inside a using block
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Start export from the Inbox folder (or any other root folder)
                string startFolderUri = client.MailboxInfo.InboxUri;
                ExportFolder(client, startFolderUri, Path.Combine(outputRoot, "Inbox"));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Recursively export all messages in a folder and its subfolders
    private static void ExportFolder(IEWSClient client, string folderUri, string localPath)
    {
        try
        {
            // Create local directory for this folder
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            // List messages in the current folder
            ExchangeMessageInfoCollection messages = client.ListMessages(folderUri);
            foreach (ExchangeMessageInfo messageInfo in messages)
            {
                try
                {
                    // Fetch the full message
                    MailMessage message = client.FetchMessage(messageInfo.UniqueUri);

                    // Build a safe file name from the subject
                    string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }
                    string fileName = subject + ".msg";
                    string filePath = Path.Combine(localPath, fileName);

                    // Save the message as MSG using a verified SaveOptions overload
                    message.Save(filePath, Aspose.Email.SaveOptions.DefaultMsg);
                }
                catch (Exception exMessage)
                {
                    Console.Error.WriteLine($"Failed to export message: {exMessage.Message}");
                }
            }

            // List subfolders and recurse
            ExchangeFolderInfoCollection subfolders = client.ListSubFolders(folderUri);
            foreach (ExchangeFolderInfo subfolder in subfolders)
            {
                // Use the folder's Uri property (verified) for recursion
                string subfolderLocalPath = Path.Combine(localPath, subfolder.DisplayName);
                ExportFolder(client, subfolder.Uri, subfolderLocalPath);
            }
        }
        catch (Exception exFolder)
        {
            Console.Error.WriteLine($"Failed to process folder '{folderUri}': {exFolder.Message}");
        }
    }
}