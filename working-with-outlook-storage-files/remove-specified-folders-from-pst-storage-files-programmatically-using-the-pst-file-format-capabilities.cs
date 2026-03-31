using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Placeholder PST created with default root folder.
                    }
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                }
                return;
            }

            // Open the existing PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // List of folder names to be removed.
                string[] foldersToDelete = { "FolderA", "FolderB" };

                foreach (string folderName in foldersToDelete)
                {
                    try
                    {
                        // Attempt to retrieve the subfolder.
                        FolderInfo folder = pst.RootFolder.GetSubFolder(folderName);
                        if (folder != null)
                        {
                            // Delete the folder using its entry identifier.
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
                        Console.Error.WriteLine($"Error processing folder '{folderName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
