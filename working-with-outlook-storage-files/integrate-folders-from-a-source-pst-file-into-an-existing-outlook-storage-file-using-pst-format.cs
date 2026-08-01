using Aspose.Email.Mapi;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            const string sourcePstPath = "source.pst";
            const string targetPstPath = "target.pst";
            const string outputPstPath = "target_merged.pst";

            // Create placeholder PST files if they do not exist
            if (!File.Exists(sourcePstPath))
            {
                using (PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode)) { }
                Console.Error.WriteLine($"Source PST not found. Created empty placeholder at '{sourcePstPath}'.");
            }

            if (!File.Exists(targetPstPath))
            {
                using (PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode)) { }
                Console.Error.WriteLine($"Target PST not found. Created empty placeholder at '{targetPstPath}'.");
            }

            // Open source and target PST files
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
            {
                // Iterate through each top‑level folder in the source PST
                foreach (FolderInfo sourceFolder in sourcePst.RootFolder.GetSubFolders())
                {
                    // Find existing folder in target PST or create a new one
                    FolderInfo targetFolder = targetPst.RootFolder.GetSubFolder(sourceFolder.DisplayName);
                    if (targetFolder == null)
                    {
                        targetFolder = targetPst.RootFolder.AddSubFolder(sourceFolder.DisplayName);
                    }

                    // Copy all messages from source folder to target folder
                    MessageInfoCollection sourceMessages = sourceFolder.GetContents();
                    foreach (MessageInfo msgInfo in sourceMessages)
                    {
                        MapiMessage message = sourcePst.ExtractMessage(msgInfo);
                        targetFolder.AddMessage(message);
                    }
                }

                // Save merged PST to a new file (different from the opened target file)
                targetPst.SaveAs(outputPstPath, FileFormat.Pst);
                Console.WriteLine($"Merged PST saved to '{outputPstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
