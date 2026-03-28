using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                SearchFolder(pst, rootFolder, "Invoice");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void SearchFolder(PersonalStorage pst, FolderInfo folder, string keyword)
    {
        Console.WriteLine($"Folder: {folder.DisplayName}");

        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            if (!string.IsNullOrEmpty(messageInfo.Subject) && messageInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"Found message with subject: {messageInfo.Subject}");
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    string bodyPreview = string.Empty;
                    if (!string.IsNullOrEmpty(mapiMessage.Body))
                    {
                        int previewLength = Math.Min(100, mapiMessage.Body.Length);
                        bodyPreview = mapiMessage.Body.Substring(0, previewLength);
                    }
                    Console.WriteLine($"Body preview: {bodyPreview}");
                }
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            SearchFolder(pst, subFolder, keyword);
        }
    }
}
