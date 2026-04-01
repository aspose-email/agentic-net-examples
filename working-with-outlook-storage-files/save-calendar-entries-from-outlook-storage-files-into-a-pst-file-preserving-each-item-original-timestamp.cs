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
            // Input Outlook storage file (PST/OST). Adjust the path as needed.
            string sourcePath = "source.pst";
            // Destination PST file where calendar entries will be saved.
            string destinationPath = "destination.pst";

            // Guard source file existence.
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Error: Source file not found – {sourcePath}");
                // Create a minimal placeholder PST to allow the example to continue.
                using (PersonalStorage placeholder = PersonalStorage.Create(sourcePath, FileFormatVersion.Unicode))
                {
                    // No content needed.
                }
                Console.Error.WriteLine("Created a placeholder source PST.");
                return;
            }

            // Ensure destination directory exists.
            string destDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Open source and destination PST files.
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePath))
            using (PersonalStorage destPst = PersonalStorage.Create(destinationPath, FileFormatVersion.Unicode))
            {
                // Start copying from the root folder.
                FolderInfo sourceRoot = sourcePst.RootFolder;
                FolderInfo destRoot = destPst.RootFolder;

                CopyFolderRecursive(sourcePst, sourceRoot, destPst, destRoot);
            }

            Console.WriteLine("Calendar entries have been saved to the destination PST.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively copies folders and messages, preserving original timestamps.
    private static void CopyFolderRecursive(PersonalStorage srcPst, FolderInfo srcFolder,
                                            PersonalStorage dstPst, FolderInfo dstParentFolder)
    {
        // Create or get the corresponding folder in the destination PST.
        FolderInfo dstFolder;
        try
        {
            dstFolder = dstParentFolder.GetSubFolder(srcFolder.DisplayName);
        }
        catch
        {
            // Subfolder does not exist; create it.
            dstFolder = dstParentFolder.AddSubFolder(srcFolder.DisplayName);
        }

        // Copy all messages from the source folder to the destination folder.
        foreach (MessageInfo msgInfo in srcFolder.EnumerateMessages())
        {
            // Extract the full MAPI message.
            using (MapiMessage message = srcPst.ExtractMessage(msgInfo))
            {
                // Add the message to the destination folder.
                // AddMessage preserves the internal properties, including timestamps.
                dstFolder.AddMessage(message);
            }
        }

        // Recursively process subfolders.
        foreach (FolderInfo subFolder in srcFolder.GetSubFolders())
        {
            CopyFolderRecursive(srcPst, subFolder, dstPst, dstFolder);
        }
    }
}
