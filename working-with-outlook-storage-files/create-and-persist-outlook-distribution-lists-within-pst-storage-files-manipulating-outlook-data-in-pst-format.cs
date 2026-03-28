using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "distributionlist.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(Path.GetFullPath(pstPath));
            if (!Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create a new PST file (overwrite if it already exists)
            if (File.Exists(pstPath))
            {
                File.Delete(pstPath);
            }

            // Create the PST with Unicode format
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Create a distribution list
                MapiDistributionList distributionList = new MapiDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Add members to the distribution list
                distributionList.Members.Add(new MapiDistributionListMember("Alice", "alice@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Bob", "bob@example.com"));

                // Convert the distribution list to an underlying MapiMessage
                using (MapiMessage dlMessage = distributionList.GetUnderlyingMessage())
                {
                    // Add the message to the root folder of the PST
                    string entryId = pst.RootFolder.AddMessage(dlMessage);
                    Console.WriteLine($"Distribution list added to PST. EntryId: {entryId}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
