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
            // Path to the PST file
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create a new PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // PST created – no additional actions required here
                }
            }

            // Open the PST file for read/write operations
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the root folder of the PST
                FolderInfo rootFolder = pst.RootFolder;

                // -------------------- Create Distribution List --------------------
                MapiDistributionList distributionList = new MapiDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Add initial members
                distributionList.Members.Add(new MapiDistributionListMember("John Doe", "john@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Alice Smith", "alice@example.com"));

                // Add the distribution list to the PST and obtain its EntryId
                string entryId = rootFolder.AddMapiMessageItem(distributionList);
                Console.WriteLine($"Created distribution list. EntryId: {entryId}");

                // -------------------- Delete Distribution List --------------------
                pst.DeleteItem(entryId);
                Console.WriteLine($"Deleted distribution list. EntryId: {entryId}");

                // -------------------- Modify (Re‑create) Distribution List --------------------
                MapiDistributionList modifiedList = new MapiDistributionList();
                modifiedList.DisplayName = "Sample Distribution List (Modified)";

                // Add members, including a new one
                modifiedList.Members.Add(new MapiDistributionListMember("John Doe", "john@example.com"));
                modifiedList.Members.Add(new MapiDistributionListMember("Alice Smith", "alice@example.com"));
                modifiedList.Members.Add(new MapiDistributionListMember("Bob Johnson", "bob@example.com"));

                // Add the modified distribution list to the PST
                string modifiedEntryId = rootFolder.AddMapiMessageItem(modifiedList);
                Console.WriteLine($"Created modified distribution list. EntryId: {modifiedEntryId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
