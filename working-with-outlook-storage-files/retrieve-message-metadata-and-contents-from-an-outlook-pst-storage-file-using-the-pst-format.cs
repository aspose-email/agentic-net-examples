using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "storage.pst";

                // Verify PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Retrieve total items count
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items count: {totalItems}");

                    // Iterate through each subfolder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        // Enumerate messages in the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                            Console.WriteLine($"From: {messageInfo.SenderRepresentativeName}");

                            // Extract the full message
                            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                            {
                                // Print a preview of the body (first 100 characters)
                                if (!string.IsNullOrEmpty(msg.Body))
                                {
                                    int previewLength = Math.Min(100, msg.Body.Length);
                                    Console.WriteLine($"Body preview: {msg.Body.Substring(0, previewLength)}");
                                }

                                // Create a safe filename based on the subject
                                string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : string.Concat(msg.Subject.Split(Path.GetInvalidFileNameChars()));
                                string msgFilePath = $"{safeSubject}.msg";

                                // Save the message as a .msg file
                                try
                                {
                                    msg.Save(msgFilePath);
                                    Console.WriteLine($"Saved message to {msgFilePath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Error saving message: {saveEx.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
