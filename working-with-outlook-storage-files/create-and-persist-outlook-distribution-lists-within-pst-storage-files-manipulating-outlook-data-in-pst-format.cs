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
            string pstPath = "SampleDistributionLists.pst";

            // Ensure the directory for the PST file exists
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
                // Use the root folder to store the distribution list
                FolderInfo rootFolder = pst.RootFolder;

                // Create a new distribution list
                MapiDistributionList distributionList = new MapiDistributionList
                {
                    DisplayName = "Sample Distribution List"
                };

                // Add members to the distribution list
                distributionList.Members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Jane Smith", "jane.smith@example.com"));

                // Add the distribution list to the PST folder
                try
                {
                    string entryId = rootFolder.AddMapiMessageItem(distributionList);
                    Console.WriteLine($"Distribution list added with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding distribution list to PST: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
