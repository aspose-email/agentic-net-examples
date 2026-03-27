using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Root folder is created automatically.
                }
            }

            // Open the PST file and work with a FolderInfo instance.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Create a new subfolder under the root.
                FolderInfo folder = pst.RootFolder.AddSubFolder("OriginalFolder");

                // Assign a new display name to the folder.
                folder.ChangeDisplayName("NewDisplayName");

                // Verify the change.
                Console.WriteLine("Folder display name: " + folder.DisplayName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
