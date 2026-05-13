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
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine("Placeholder PST file created at: " + pstPath);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine("Failed to create placeholder PST: " + createEx.Message);
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all subfolders of the root folder.
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        // Determine the predefined type of the current folder.
                        StandardIpmFolder predefinedType = folderInfo.GetPredefinedType(false);

                        if (predefinedType != StandardIpmFolder.Unspecified)
                        {
                            Console.WriteLine($"Folder \"{folderInfo.DisplayName}\" is a predefined folder of type: {predefinedType}");
                        }
                        else
                        {
                            Console.WriteLine($"Folder \"{folderInfo.DisplayName}\" is not a predefined folder.");
                        }
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine("Error accessing PST file: " + pstEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
