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

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                Console.WriteLine($"Placeholder PST created at: {pstPath}");
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Start enumeration from the root folder
                EnumerateFolder(pst, pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively enumerate messages in a folder and its subfolders
    static void EnumerateFolder(PersonalStorage pst, FolderInfo folder)
    {
        // Process each message in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Extract the full MAPI message from the PST
            using (MapiMessage message = pst.ExtractMessage(messageInfo))
            {
                // Identify distribution list items by message class
                if (string.Equals(message.MessageClass, "IPM.DistList", StringComparison.OrdinalIgnoreCase))
                {
                    // Convert the MAPI message to a distribution list object
                    MapiDistributionList distributionList = (MapiDistributionList)message.ToMapiMessageItem();

                    Console.WriteLine($"Distribution List: {distributionList.DisplayName}");

                    // Enumerate and display each member of the distribution list
                    foreach (MapiDistributionListMember member in distributionList.Members)
                    {
                        Console.WriteLine($"  Member: {member.DisplayName} <{member.EmailAddress}>");
                    }
                }
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            EnumerateFolder(pst, subFolder);
        }
    }
}
