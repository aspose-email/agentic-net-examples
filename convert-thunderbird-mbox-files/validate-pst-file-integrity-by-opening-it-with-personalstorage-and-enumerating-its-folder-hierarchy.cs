using Aspose.Email;
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

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    // Add a default folder to make the PST usable.
                    createdPst.RootFolder.AddSubFolder("Inbox");
                    createdPst.Dispose();
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file and enumerate its folder hierarchy.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                Console.WriteLine($"Opened PST: {pstPath}");
                EnumerateFolder(pst.RootFolder, 0);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void EnumerateFolder(FolderInfo folder, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 2);
        Console.WriteLine($"{indent}Folder: {folder.DisplayName}");
        Console.WriteLine($"{indent}  Items: {folder.ContentCount}, Unread: {folder.ContentUnreadCount}");

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            EnumerateFolder(subFolder, indentLevel + 1);
        }
    }
}
