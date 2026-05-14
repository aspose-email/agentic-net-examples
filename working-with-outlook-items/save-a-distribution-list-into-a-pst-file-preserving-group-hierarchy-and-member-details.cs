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
            string pstPath = "distribution_list.pst";

            // Ensure the PST file exists; create a new one if it does not.
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

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get or create the Contacts folder.
                FolderInfo contactsFolder;
                try
                {
                    contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                }
                catch (Exception)
                {
                    // If the predefined folder is missing, create it.
                    contactsFolder = pst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                }

                // Create a distribution list.
                MapiDistributionList distributionList = new MapiDistributionList
                {
                    DisplayName = "Team Members"
                };

                // Add members to the distribution list.
                distributionList.Members.Add(new MapiDistributionListMember("Alice Smith", "alice@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Bob Johnson", "bob@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Carol Davis", "carol@example.com"));

                // Add the distribution list to the Contacts folder.
                try
                {
                    contactsFolder.AddMapiMessageItem(distributionList);
                    Console.WriteLine("Distribution list saved to PST successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add distribution list: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
