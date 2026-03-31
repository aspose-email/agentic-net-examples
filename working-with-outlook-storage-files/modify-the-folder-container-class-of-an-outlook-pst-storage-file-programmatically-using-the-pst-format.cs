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

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default folder to work with.
                        pstCreate.RootFolder.AddSubFolder("TargetFolder");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the existing PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Locate the folder named "TargetFolder".
                    FolderInfo targetFolder = null;
                    foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                    {
                        if (string.Equals(subFolder.DisplayName, "TargetFolder", StringComparison.OrdinalIgnoreCase))
                        {
                            targetFolder = subFolder;
                            break;
                        }
                    }

                    // If the folder does not exist, create it.
                    if (targetFolder == null)
                    {
                        targetFolder = pst.RootFolder.AddSubFolder("TargetFolder");
                    }

                    // Change the container class of the folder.
                    // Example container class: "IPF.Note" (standard mail item).
                    targetFolder.ChangeContainerClass("IPF.Note");

                    Console.WriteLine($"Container class of folder '{targetFolder.DisplayName}' has been changed.");
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
}
