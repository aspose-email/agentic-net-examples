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
            // Paths for PST file
            string pstPath = "sample.pst";

            // Ensure the directory for the PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create a new PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Ensure we can write to the PST
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("PST file is read‑only.");
                    return;
                }

                // Get (or create) a folder to store distribution lists
                FolderInfo dlFolder;
                try
                {
                    dlFolder = pst.RootFolder.GetSubFolder("DistributionLists");
                }
                catch
                {
                    dlFolder = pst.RootFolder.AddSubFolder("DistributionLists");
                }

                // -------------------------
                // Create a new distribution list
                // -------------------------
                MapiDistributionList newDl = new MapiDistributionList();
                newDl.DisplayName = "Sample Distribution List";

                // Add members to the distribution list
                MapiDistributionListMember member1 = new MapiDistributionListMember("John Doe", "john.doe@example.com");
                MapiDistributionListMember member2 = new MapiDistributionListMember("Jane Smith", "jane.smith@example.com");
                newDl.Members.Add(member1);
                newDl.Members.Add(member2);

                // Convert the distribution list to a MapiMessage and add it to the folder
                MapiMessage dlMessage = newDl.GetUnderlyingMessage();
                string entryId = dlFolder.AddMessage(dlMessage);
                Console.WriteLine($"Created distribution list with EntryId: {entryId}");

                // -------------------------
                // Modify the distribution list (add another member)
                // -------------------------
                // Extract the message we just added
                MapiMessage extractedMessage = pst.ExtractMessage(entryId);
                // Cast to distribution list
                MapiDistributionList extractedDl = (MapiDistributionList)extractedMessage.ToMapiMessageItem();

                // Add a new member
                MapiDistributionListMember member3 = new MapiDistributionListMember("Bob Lee", "bob.lee@example.com");
                extractedDl.Members.Add(member3);
                extractedDl.DisplayName = "Sample Distribution List (Updated)";

                // Save the modified distribution list back to the PST (replace the old one)
                // Delete the old entry first
                pst.DeleteItem(entryId);
                // Add the updated message
                MapiMessage updatedMessage = extractedDl.GetUnderlyingMessage();
                string updatedEntryId = dlFolder.AddMessage(updatedMessage);
                Console.WriteLine($"Modified distribution list, new EntryId: {updatedEntryId}");

                // -------------------------
                // Delete the distribution list
                // -------------------------
                pst.DeleteItem(updatedEntryId);
                Console.WriteLine("Deleted the distribution list.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
