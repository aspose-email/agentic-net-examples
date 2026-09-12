using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "storage.pst";
            const string outputDir = "ExtractedMessages";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                // Create a new Unicode PST file
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created new PST file at '{pstPath}'.");
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Total items in the PST store
                int totalItemsCount = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");

                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        try
                        {
                            // Extract the MAPI message
                            using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                            {
                                // Convert to MailMessage for easier handling/saving
                                using (MailMessage msg = mapiMsg.ToMailMessage(new MailConversionOptions()))
                                {
                                    // Build a safe filename from the subject
                                    string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "NoSubject" : msg.Subject;
                                    foreach (char c in Path.GetInvalidFileNameChars())
                                    {
                                        safeSubject = safeSubject.Replace(c, '_');
                                    }

                                    string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");
                                    msg.Save(msgPath);
                                    Console.WriteLine($"Saved message to '{msgPath}'.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract/save message '{messageInfo.Subject}': {ex.Message}");
                        }
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
