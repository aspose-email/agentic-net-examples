using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths (adjust as needed)
            string pstFilePath = "Existing.pst";
            string sourceFolderPath = "EmailsToImport";
            string targetFolderName = "Imported";

            // Ensure source folder exists
            if (!Directory.Exists(sourceFolderPath))
            {
                Console.Error.WriteLine($"Error: Source folder not found – {sourceFolderPath}");
                return;
            }

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    using (PersonalStorage created = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        // PST created successfully
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the existing PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Locate the target folder; create it if it does not exist
                FolderInfo targetFolder = null;
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    if (string.Equals(folder.DisplayName, targetFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetFolder = folder;
                        break;
                    }
                }

                if (targetFolder == null)
                {
                    try
                    {
                        targetFolder = pst.RootFolder.AddSubFolder(targetFolderName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating folder '{targetFolderName}': {ex.Message}");
                        return;
                    }
                }

                // Process each .msg file in the source folder
                string[] messageFiles;
                try
                {
                    messageFiles = Directory.GetFiles(sourceFolderPath, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error enumerating files: {ex.Message}");
                    return;
                }

                foreach (string msgPath in messageFiles)
                {
                    if (!File.Exists(msgPath))
                    {
                        Console.Error.WriteLine($"Skipping missing file: {msgPath}");
                        continue;
                    }

                    try
                    {
                        using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                        {
                            string entryId = targetFolder.AddMessage(mapiMessage);
                            Console.WriteLine($"Added: {Path.GetFileName(msgPath)} – EntryId: {entryId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add message '{msgPath}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
