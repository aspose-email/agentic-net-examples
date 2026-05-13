using System;
using System.Collections.Generic;
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
            string pstPath = "archive.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                HashSet<string> messageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                bool duplicateFound = false;

                // Process messages in the root folder
                foreach (MessageInfo msgInfo in pst.RootFolder.EnumerateMessages())
                {
                    ProcessMessage(pst, msgInfo, messageIds, ref duplicateFound);
                }

                // Recursively process subfolders
                ProcessSubFolders(pst.RootFolder, pst, messageIds, ref duplicateFound);

                if (!duplicateFound)
                {
                    Console.WriteLine("All transport message IDs are unique.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessMessage(PersonalStorage pst, MessageInfo msgInfo, HashSet<string> ids, ref bool duplicateFound)
    {
        try
        {
            using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
            {
                string transportId = mapiMsg.InternetMessageId ?? string.Empty;

                if (string.IsNullOrEmpty(transportId))
                {
                    Console.WriteLine("Message without Transport Message ID encountered.");
                    return;
                }

                if (!ids.Add(transportId))
                {
                    Console.WriteLine($"Duplicate Transport Message ID detected: {transportId}");
                    duplicateFound = true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing a message: {ex.Message}");
        }
    }

    // Recursively process subfolders
    private static void ProcessSubFolders(FolderInfo folder, PersonalStorage pst, HashSet<string> ids, ref bool duplicateFound)
    {
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            foreach (MessageInfo msgInfo in subFolder.EnumerateMessages())
            {
                ProcessMessage(pst, msgInfo, ids, ref duplicateFound);
            }

            // Recurse into deeper subfolders
            ProcessSubFolders(subFolder, pst, ids, ref duplicateFound);
        }
    }
}
