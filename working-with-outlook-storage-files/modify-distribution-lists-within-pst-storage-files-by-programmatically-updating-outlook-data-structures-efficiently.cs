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
            const string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a simple distribution list with one member.
                        MapiDistributionList dl = new MapiDistributionList
                        {
                            DisplayName = "Sample Distribution List"
                        };
                        dl.Members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));

                        // Add the distribution list to the root folder.
                        string entryId = pstCreate.RootFolder.AddMapiMessageItem(dl);
                        Console.WriteLine($"Created PST and added distribution list (EntryId: {entryId}).");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the existing PST file for read/write.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                bool listFound = false;

                // Enumerate messages in the root folder to locate a distribution list.
                foreach (MessageInfo msgInfo in rootFolder.EnumerateMessages())
                {
                    // Extract the message as a MapiMessage.
                    using (MapiMessage msg = pst.ExtractMessage(msgInfo))
                    {
                        if (msg.SupportedType == MapiItemType.DistList)
                        {
                            listFound = true;
                            // Convert to a MapiDistributionList for editing.
                            MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem();

                            // Add a new member to the distribution list.
                            distList.Members.Add(new MapiDistributionListMember("Jane Smith", "jane.smith@example.com"));
                            Console.WriteLine("Added new member to distribution list.");

                            // Update the message in the PST.
                            rootFolder.UpdateMessage(msgInfo.EntryIdString, distList);
                            Console.WriteLine("Distribution list updated in PST.");
                            break;
                        }
                    }
                }

                if (!listFound)
                {
                    Console.WriteLine("No distribution list found in the PST.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }
}
