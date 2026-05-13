using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            if (!File.Exists(pstPath))
            {
                // Create a minimal placeholder PST file
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Placeholder PST created at: {pstPath}");
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                var folderMessageCounts = new Dictionary<string, int>();

                void ProcessFolder(FolderInfo folder)
                {
                    int messageCount = folder.ContentCount;
                    folderMessageCounts[folder.DisplayName] = messageCount;

                    foreach (FolderInfo subFolder in folder.GetSubFolders())
                    {
                        ProcessFolder(subFolder);
                    }
                }

                ProcessFolder(pst.RootFolder);

                foreach (var entry in folderMessageCounts)
                {
                    Console.WriteLine($"Folder: {entry.Key}, Message Count: {entry.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
