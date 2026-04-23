using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard file existence for the source MBOX
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the target PST does not already exist; if it does, delete it to allow recreation
            try
            {
                if (File.Exists(pstPath))
                {
                    File.Delete(pstPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST file: {ex.Message}");
                return;
            }

            // Convert MBOX to PST using the static MailStorageConverter class
            PersonalStorage pst = null;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Log folder information after conversion
            using (pst)
            {
                // Root folder
                FolderInfo rootFolder = pst.RootFolder;
                Console.WriteLine($"Folder: {rootFolder.DisplayName}");
                Console.WriteLine($"Total items: {rootFolder.ContentCount}");
                Console.WriteLine($"Total unread items: {rootFolder.ContentUnreadCount}");

                // Subfolders
                foreach (FolderInfo subFolder in rootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {subFolder.DisplayName}");
                    Console.WriteLine($"Total items: {subFolder.ContentCount}");
                    Console.WriteLine($"Total unread items: {subFolder.ContentUnreadCount}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
