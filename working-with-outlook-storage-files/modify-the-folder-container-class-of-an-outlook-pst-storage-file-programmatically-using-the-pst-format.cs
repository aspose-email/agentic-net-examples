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

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Access the root folder.
                    FolderInfo rootFolder = pst.RootFolder;

                    // Locate or create a subfolder named "TargetFolder".
                    FolderInfo targetFolder = null;
                    foreach (FolderInfo subFolder in rootFolder.GetSubFolders())
                    {
                        if (subFolder.DisplayName == "TargetFolder")
                        {
                            targetFolder = subFolder;
                            break;
                        }
                    }

                    if (targetFolder == null)
                    {
                        // Create the subfolder.
                        rootFolder.AddSubFolder("TargetFolder");
                        // Retrieve the newly created folder.
                        targetFolder = rootFolder.GetSubFolder("TargetFolder");
                    }

                    // Modify the container class of the target folder.
                    targetFolder.ChangeContainerClass("IPF.Note");

                    Console.WriteLine("Container class of 'TargetFolder' has been updated.");
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
