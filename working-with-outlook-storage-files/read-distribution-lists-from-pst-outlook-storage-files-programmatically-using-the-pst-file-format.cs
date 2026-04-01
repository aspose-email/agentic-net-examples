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
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default Contacts folder (optional)
                        createdPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file and read distribution lists
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder
                ProcessFolder(pst.RootFolder, pst);

                // Recursively process subfolders
                foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolderRecursive(subFolder, pst);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Process a single folder's messages
    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst)
    {
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
            {
                if (msg.SupportedType == MapiItemType.DistList)
                {
                    // Convert to a distribution list object
                    MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem();

                    Console.WriteLine($"Distribution List: {distList.DisplayName}");
                    Console.WriteLine($"Members count: {distList.Members.Count}");

                    foreach (MapiDistributionListMember member in distList.Members)
                    {
                        Console.WriteLine($" - {member.DisplayName} <{member.EmailAddress}>");
                    }

                    Console.WriteLine();
                }
            }
        }
    }

    // Recursively process subfolders
    private static void ProcessFolderRecursive(FolderInfo folder, PersonalStorage pst)
    {
        ProcessFolder(folder, pst);

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolderRecursive(subFolder, pst);
        }
    }
}
