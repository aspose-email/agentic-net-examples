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
            // Define PST file path
            string pstPath = "MyDistributionList.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Contacts folder (standard predefined folder)
                FolderInfo contactsFolder;
                try
                {
                    contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Contacts folder: {ex.Message}");
                    return;
                }

                // Prepare distribution list members
                MapiDistributionListMemberCollection members = new MapiDistributionListMemberCollection();
                members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));
                members.Add(new MapiDistributionListMember("Jane Smith", "jane.smith@example.com"));

                // Create the distribution list
                MapiDistributionList distributionList = new MapiDistributionList("My Distribution List", members);

                // Convert distribution list to a MAPI message
                MapiMessage distMessage = distributionList.GetUnderlyingMessage();

                // Add the distribution list message to the Contacts folder
                try
                {
                    string entryId = contactsFolder.AddMessage(distMessage);
                    Console.WriteLine($"Distribution list added with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add distribution list to PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
