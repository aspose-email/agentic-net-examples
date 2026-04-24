using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping backup operation.");
                return;
            }

            // Output directory for exported .eml files
            string outputRoot = Path.Combine(Environment.CurrentDirectory, "ImapBackup");
            try
            {
                if (!Directory.Exists(outputRoot))
                {
                    Directory.CreateDirectory(outputRoot);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Selected folders to backup
            string[] selectedFolderNames = new[] { "INBOX", "Sent Items" };

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Iterate over each selected folder
                    foreach (string folderName in selectedFolderNames)
                    {
                        try
                        {
                            // Select the folder on the server
                            client.SelectFolder(folderName);

                            // Retrieve list of messages in the folder
                            ImapMessageInfoCollection messageInfos = client.ListMessages();

                            // Prepare folder-specific output path
                            string folderOutputPath = Path.Combine(outputRoot, folderName);
                            if (!Directory.Exists(folderOutputPath))
                            {
                                Directory.CreateDirectory(folderOutputPath);
                            }

                            // Export each message to an .eml file
                            foreach (ImapMessageInfo messageInfo in messageInfos)
                            {
                                try
                                {
                                    // Fetch the full message
                                    MailMessage message = client.FetchMessage(messageInfo.UniqueId);

                                    // Build file name using unique identifier
                                    string emlFilePath = Path.Combine(folderOutputPath, $"{messageInfo.UniqueId}.eml");

                                    // Save the message as .eml
                                    using (FileStream fs = new FileStream(emlFilePath, FileMode.Create, FileAccess.Write))
                                    {
                                        message.Save(fs, SaveOptions.DefaultEml);
                                    }
                                }
                                catch (Exception exMessage)
                                {
                                    Console.Error.WriteLine($"Failed to export message UID {messageInfo.UniqueId} from folder '{folderName}': {exMessage.Message}");
                                    // Continue with next message
                                }
                            }
                        }
                        catch (Exception exFolder)
                        {
                            Console.Error.WriteLine($"Failed to process folder '{folderName}': {exFolder.Message}");
                            // Continue with next folder
                        }
                    }
                }
                catch (Exception exClient)
                {
                    Console.Error.WriteLine($"IMAP client operation failed: {exClient.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
