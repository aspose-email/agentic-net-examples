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

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each folder in the PST
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        // Extract the full MAPI message
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Check if the message represents a distribution list
                            if (mapiMessage.SupportedType == MapiItemType.DistList)
                            {
                                // Convert to a MapiDistributionList object
                                MapiDistributionList distributionList = (MapiDistributionList)mapiMessage.ToMapiMessageItem();

                                Console.WriteLine($"Distribution List: {distributionList.DisplayName}");
                                Console.WriteLine($"Members count: {distributionList.Members.Count}");

                                // List each member's details
                                foreach (MapiDistributionListMember member in distributionList.Members)
                                {
                                    Console.WriteLine($"  Name: {member.DisplayName}, Email: {member.EmailAddress}");
                                }

                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
