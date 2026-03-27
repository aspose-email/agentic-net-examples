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
                string pstPath = "sample.pst";

                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                    return;
                }

                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items count: {totalItems}");

                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                            {
                                string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : msg.Subject;
                                string fileName = $"{safeSubject}.msg";

                                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                {
                                    fileName = fileName.Replace(invalidChar, '_');
                                }

                                msg.Save(fileName);
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
}
