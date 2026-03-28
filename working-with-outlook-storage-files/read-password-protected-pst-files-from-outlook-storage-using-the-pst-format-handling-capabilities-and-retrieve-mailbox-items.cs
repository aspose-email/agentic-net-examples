using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "protected.pst";
            string password = "secret";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    MessageStore store = pst.Store;

                    if (store.IsPasswordProtected)
                    {
                        if (!store.IsPasswordValid(password))
                        {
                            Console.Error.WriteLine("Error: Invalid password for the PST file.");
                            return;
                        }
                    }

                    Console.WriteLine($"Total items in store: {store.GetTotalItemsCount()}");

                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folder.DisplayName}");
                        Console.WriteLine($"  Total items: {folder.ContentCount}");
                        Console.WriteLine($"  Unread items: {folder.ContentUnreadCount}");

                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            Console.WriteLine($"  Subject: {messageInfo.Subject}");

                            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                            {
                                string safeSubject = MakeFileNameSafe(msg.Subject);
                                string fileName = $"{safeSubject}.msg";
                                msg.Save(fileName);
                                Console.WriteLine($"    Saved as: {fileName}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static string MakeFileNameSafe(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "Untitled";

        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
