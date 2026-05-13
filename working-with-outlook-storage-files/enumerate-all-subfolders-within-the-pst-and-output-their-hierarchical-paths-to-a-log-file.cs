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
            string logPath = "pst_folders.log";

            // Verify PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure log directory exists
            try
            {
                string logDir = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {dirEx.Message}");
                return;
            }

            // Open PST and write folder hierarchy to log
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                using (StreamWriter writer = new StreamWriter(logPath, false))
                {
                    FolderInfo rootFolder = pst.RootFolder;
                    WriteFolderPath(rootFolder, writer);
                    EnumerateSubFolders(rootFolder, writer);
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively enumerate subfolders and write their full paths
    private static void EnumerateSubFolders(FolderInfo folder, StreamWriter writer)
    {
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            WriteFolderPath(subFolder, writer);
            EnumerateSubFolders(subFolder, writer);
        }
    }

    // Write a single folder's full path to the log
    private static void WriteFolderPath(FolderInfo folder, StreamWriter writer)
    {
        try
        {
            string fullPath = folder.RetrieveFullPath();
            writer.WriteLine(fullPath);
        }
        catch (Exception pathEx)
        {
            Console.Error.WriteLine($"Failed to retrieve path for folder '{folder?.DisplayName}': {pathEx.Message}");
        }
    }
}
