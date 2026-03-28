using System;
using System.IO;
using Aspose.Email.Storage.Pst;

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

            string[] foldersToDelete = new string[] { "FolderA", "FolderB" };

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    foreach (string folderName in foldersToDelete)
                    {
                        try
                        {
                            FolderInfo folder = pst.RootFolder.GetSubFolder(folderName);
                            if (folder != null)
                            {
                                pst.DeleteItem(folder.EntryIdString);
                                Console.WriteLine($"Deleted folder: {folderName}");
                            }
                            else
                            {
                                Console.WriteLine($"Folder not found: {folderName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error deleting folder '{folderName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
